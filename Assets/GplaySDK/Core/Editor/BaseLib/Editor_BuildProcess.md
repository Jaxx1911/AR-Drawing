Table: Validate

| Assembly Name       | Class Name                         | Method Name | Order  | Description                      |
|:--------------------|:-----------------------------------|:------------|:-------|:---------------------------------|
| Core.Editor.BaseLib | EditorInitialize_Validator         | Validator   | -10000 | Validate all EditorInitialize    |
| Core.Editor.BaseLib | RuntimeInitializer_Validator       | Validator   | -1000  | Validate all Runtime Initializer |
| Core.Editor.BaseLib | RequirePropertyAttribute_Validator | Validator   | -950   | Validate Require Property        |
| -                   | -                                  | -           | 0      | -                                |
|                     |                                    |             |        |                                  |

Table: Symbol

| Assembly Name       | Class Name            | Method Name       | Order | Description                                 |
|:--------------------|:----------------------|:------------------|:------|:--------------------------------------------|
| Core.Editor         | GplaySDKConfig_Editor | SymbolInitializer | -1000 |                                             |
| Core.Editor.BaseLib | BasicSymbolInitialize | Initialize        | -950  | Add Symbol for any SDK module are installed |
| -                   | -                     | -                 | 0     | -                                           |
|                     |                       |                   |       |                                             |

Table: Asmdef

| Assembly Name | Class Name      | Method Name | Order | Description                             |
|:--------------|:----------------|:------------|:------|:----------------------------------------|
|               |                 |             |       |                                         |
| -             | -               | -           | 0     | -                                       |
| Core.Editor   | Asm_Initializer | Initialize  | 1000  | Include installed module to Core module |

Table: Compile

| Assembly Name                       | Class Name                | Method Name | Order | Description                                                  |
|:------------------------------------|:--------------------------|:------------|:------|:-------------------------------------------------------------|
| Core.Editor                         | GplaySDKConfig_Editor     | CreateAsset | -10   | Create Basic Config if not exists                            |
| -                                   | -                         | -           | 0     | -                                                            |
| Core.Editor.BaseLib                 | RuntimeInitialize_Compile | Compile     | 5     | Create RuntimeInitializeInfos to init in runtime             |
| Core.Editor.BaseLib                 | RequireProperty_Compile   | Compile     | 10    | Create RequirePropertyInfos to Init in runtime               |
| MediationIntegration.BaseLib.Editor | AdsIntegration_Compile    | Compile     | 15    | Create AdsInitializeInfos to Init Ads Integration in runtime |

