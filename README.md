#友盟渠道打包工具 (windows .net 4.0)

友盟渠道打包工具开放源码使用 GPL2 许可分发[**绿色版本下载地址**](https://github.com/umeng/umeng-muti-channel-build-tool/tree/master/Downloads)


常见错误见[**这里**](https://github.com/umeng/umeng-muti-channel-build-tool/wiki/%E5%B8%B8%E8%A7%81%E9%94%99%E8%AF%AF%E8%AF%B4%E6%98%8E%5BFAQ%5D)
目前不支持的Apk特性见[**这里**](https://github.com/umeng/umeng-muti-channel-build-tool/wiki/%E7%9B%AE%E5%89%8D%E6%B2%A1%E6%9C%89%E6%94%AF%E6%8C%81%E7%9A%84%E7%89%B9%E6%80%A7)
(可能会导致发布的SDK产生严重bug)。


Google 现在已经发布了最新的[**构建系统**](http://tools.android.com/tech-docs/new-build-system/user-guide)(New Building System) , 在 Android Studio 中已经支持了最新的
构建系统，如果开发者已经迁移，可以使用新的系统方面的生成渠道包，这是取代渠道打包工具的最佳方式，我们提供了一个简单的脚本见[这里](https://github.com/umeng/umeng-muti-channel-build-tool/tree/master/Gradle)。


##1. 关于本次更新

本次更新最大的改变是放弃了 V2.x 版本中通过 Apktool  反编译apk文件打包的方式，这种打包方式会对开发的apk文件做出大幅度的修改，可能会产生许多不兼容的问题，比如对jar包中包含资源的情况无法支持，对包含 .so 文件的apk兼容性也不好，而且在打包时 AndroidManifest.xml 文件中的特殊标签会丢失。为了解决这些问题减少对开发者apk文件的修改, 我们决定放弃这种方式，而采用直接编辑二进制的AndroidManifest.xml 文件的方式。这种方式只会修改 AndroidManifest.xml 文件，对于apk包中的资源文件和代码文件都不会做任何改变。如果打包不成功，生成的apk文件有问题，在测试阶段也可以快速发现，因为修改只会影响AndroidManifest.xml 相关的少量的设置。


## 2. 工具说明

[axmleditor.jar](https://github.com/ntop001/AXMLEditor) 一个AXML解析器，拥有很弱的编辑功能，工程中用来编辑二进制格式的 AndroidManifest.xml 文件.

JarSigner.jar 给 Apk 签名， `SignApk.jar`  文件是我们修改过的 `apk ` 签名工具，实现了和 ADT 中一样的签名方式.


## 3. 更新日志
### V3.0

2014-04-19

更新内容

>1. 更新了底层打包模块 
>2. 修复了 V2.x 中的若干bug
>3. 提高了打包速度和稳定性

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

