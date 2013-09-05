#友盟渠道打包工具 (windows .net 4.0)

友盟渠道打包工具开放源码使用 GPL2 许可分发[**绿色版本下载地址**](https://github.com/umeng/umeng-muti-channel-build-tool/tree/master/Downloads)


常见错误见[**这里**](https://github.com/umeng/umeng-muti-channel-build-tool/wiki/%E5%B8%B8%E8%A7%81%E9%94%99%E8%AF%AF%E8%AF%B4%E6%98%8E%5BFAQ%5D)
目前不支持的Apk特性见[**这里**](https://github.com/umeng/umeng-muti-channel-build-tool/wiki/%E7%9B%AE%E5%89%8D%E6%B2%A1%E6%9C%89%E6%94%AF%E6%8C%81%E7%9A%84%E7%89%B9%E6%80%A7)
(可能会导致发布的SDK产生严重bug)。


Google 现在已经发布了最新的[**构建系统**](http://tools.android.com/tech-docs/new-build-system/user-guide)(New Building System) , 在 Android Studio 中已经支持了最新的
构建系统，如果开发者已经迁移，可以使用新的系统方面的生成渠道包，这是取代渠道打包工具的最佳方式。


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

### V2.1

2013-07-09

更新内容

>1. 添加对 Java 环境变量的检测
>2. 添加对  Keystore , Alias 及 Password 的正确性检验
>3. 修复编译渠道没有保存导致的无法启动问题
>4. 修复 Password 中包含特殊字符导致的打包不成功的问题


### V2.0

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

