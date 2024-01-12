// Filename: ThreadUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 2:51 PM 11/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using GplaySDK.Core.BaseLib.Attribute;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class ThreadUtils
    {
        private static ThreadRunner _threadRunner;

        private static Thread _mainThread;

        private static readonly ConcurrentQueue<ActionInfo> ActionQueue = new ConcurrentQueue<ActionInfo>();

        public static void RunOnMainThread(Action action, bool isImmediate = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                action?.Invoke();
                return;
            }
#endif
#if G_LOG_LOW_LEVEL
            if (_threadRunner == null)
            {
                Debug.Log("Thread Runner is not initialized| Add to Queue but run later");
                ActionQueue.Enqueue(new ActionInfo(action, isImmediate));
                return;
            }
#endif
            if (IsOnMainThread)
            {
                action?.Invoke();
                return;
            }

            ActionQueue.Enqueue(new ActionInfo(action, isImmediate));
        }

        public static bool IsOnMainThread => Equals(Thread.CurrentThread, _mainThread);

        [RuntimeInitialize(-11000, RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void MainThreadRegister()
        {
            _mainThread = Thread.CurrentThread;
        }


        [RuntimeInitialize(-9000, RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RunnerInitializer()
        {
#if LOG_VERBOSE
            Debug.Log("Thread Runner Initializer");
#endif
            var runner = new GameObject("Thread Runner");
            Object.DontDestroyOnLoad(runner);
            _threadRunner = runner.AddComponent<ThreadRunner>();
        }

        private class ThreadRunner : MonoBehaviour
        {
#if G_LOG_LOW_LEVEL
            [ShowInInspector]
            private ActionInfo[] DebugActionQueue => _actionQueue.ToArray();
#endif
            private readonly ConcurrentQueue<ActionInfo> _actionQueue = ActionQueue;

            private int _threadCatchupSingleFrameCommand;

            private int _thisFrameCommandCount;

            private void Awake()
            {
                _threadCatchupSingleFrameCommand = BaseLibConfig.Instance.threadCatchUpSingleFrameCommand;
#if LOG_VERBOSE
                Debug.Log($"Thread Runner single frame max command: {_threadCatchupSingleFrameCommand}");
#endif
            }


            // ReSharper disable once MemberHidesStaticFromOuterClass

            private void Start()
            {
                StartCoroutine(ThreadCatchupCheck());
            }

            private IEnumerator ThreadCatchupCheck()
            {
                while (true)
                {
                    _thisFrameCommandCount = 0;
                    while (!_actionQueue.IsEmpty)
                    {
#if LOG_VERBOSE
                        "Thread Catchup Check (Not empty)".Log();
#endif
                        if (_thisFrameCommandCount > _threadCatchupSingleFrameCommand)
                        {
                            if (_actionQueue.TryPeek(out var peekInfo))
                            {
                                if (!peekInfo.IsImmediate)
                                {
#if LOG_VERBOSE
                                    "Thread Catchup Check (Too many command) - Stop".Log();
#endif
                                    yield return null;
                                }
#if LOG_VERBOSE
                                "Thread Catchup Check (Too many command) - Still Run".Log();
#endif
                            }
                        }

                        if (_actionQueue.TryDequeue(out var actionInfo))
                        {
                            _thisFrameCommandCount++;
#if LOG_VERBOSE && G_LOG_LOW_LEVEL
                            $"Thread Catchup Check (TryDequeue success) - {_thisFrameCommandCount}\n{actionInfo.StackTrace}"
                                .Log();
#endif
                            try
                            {
                                actionInfo.Action?.Invoke();
                            }
                            catch (System.Exception e)
                            {
#if LOG_ERROR
                                $"Thread Catchup Check (TryDequeue failed) - Exception\n{e}".LogError();
#endif
                            }
                        }
                        else
                        {
#if LOG_ERROR
#if G_LOG_LOW_LEVEL
                            $"Thread Catchup Check (TryDequeue failed)\n{actionInfo.StackTrace}".LogError();
#else
                            "Thread Catchup Check (TryDequeue failed)".LogError();
#endif
#endif
                        }
                    }

                    yield return null;
                }

                // ReSharper disable once IteratorNeverReturns
            }
        }

#if G_LOG_LOW_LEVEL
        [Serializable]
#endif
        private class ActionInfo
        {
            public readonly Action Action;
            public readonly bool IsImmediate;
#if G_LOG_LOW_LEVEL
            public readonly System.Diagnostics.StackTrace StackTrace;
#endif

            public ActionInfo(Action action, bool isImmediate)
            {
                Action = action;
                IsImmediate = isImmediate;
#if G_LOG_LOW_LEVEL
                StackTrace = new System.Diagnostics.StackTrace();
#endif
            }
        }
    }
}