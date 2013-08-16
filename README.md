#友盟渠道打包工具 (windows .net 4.0)

友盟渠道打包工具开放源码使用 GPL2 许可分发

##1. 工程结构

工程结构图 ：

```
- CommonTools 共用的工具类，包括对 `Apktool` , `Jarsigner` , `zipalign` 的封装
- UIControls_35   共用的UI类，对大部分控件的样式都是在这里设置的
- UmengMarket  Marekt 组件，现在还没有实现
- UmengPackage 打包组件
- UmengTools 工程主要UI，管理 UmengMarket, UmengPackage, UmengTools 三个组件
- UmengWidget 小工具组件，目前仅有解包分析的功能
```

打包工具组件：

```
- Source - Builder - ApkBuilder.cs     通过 APK 打包的 Builder 实现
                   - Builder.cs            抽象 Builder 类，提供打包的主要逻辑
                   - SourceBuilder.cs通过源码打包的 Builder 实现 (目前代码还没有实现)
                                     
         - Worker.cs 打包过程对外接口
```

##3. 打包流程

V2.0 版本仅实现了通过 `.apk` 打包的方式，本质上对  `apk` 文件进行反编译，修改 `AndroidManifest.xml` 文件后，再重新打包，我们使用的工具是开源的拆包工具 [Apktool](https://code.google.com/p/android-apktool/)


1. 将  `apktool`  添加到当前 `process` 的环境变量
2. 执行 `apktool d --no-src -f xxxx.apk temp` 拆解apk
3. 替换或者添加 `AndroidManifest.xml` 中的 友盟` channel`
4. 执行` apktool b temp  unsigned.apk` 重新打包apk
5. 执行 `SignApk.jar` 生成签名后的 apk 文件
6. 执行 `zipAlign` 生成对齐优化后的 apk 文件
7. 回到 3 替换新的渠道
8. 完成打包


使用  JarSigner.jar 给 Apk 签名， `SignApk.jar`  文件是我们修改过的 `apk ` 签名工具，实现了和 ADT 中一样的签名方式，使用如下：

```
Usage: signapk file.{keystore} keystore_password key_entry key_password
input.apk
output.apk
```


## 4. 更新日志

V2.0

2013-05-06

更新内容

>1. 取消通过源码打包的方式，直接通过apk 打包.
>2. UI 界面做了较大的改动
>3. 没有兼容旧版本的配置文件
>4. 添加了SDK 集成检测的小工具
>5. 重写签名工具
>6. 解决了一些 Apktool 相关的错误

### V1.2
>* 添加通过 `apk` 打包的功能

### V1.0
 提供基本的通过工程源码打包功能

