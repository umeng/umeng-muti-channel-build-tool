基于 Gradle 的多渠道打包脚本


这份脚本来自：http://my.oschina.net/uboluo/blog/157483 菠萝同学提供的打包方式，把 build.gradle 复制到Eclipse工程的根目录下面。
然后在命令行输入：

```
> cd [project_dir]           //cd 到工程根目录
> android update project -p .//生成一些配置文件，`android` 命令是在 `[Android SDK]\tools\android.bat` 提供的
> gradle build               //生成渠道包，会在 `[project_dir]\build\apk\*` 生成Apk
```

这里简单讲解一下多渠道打包的过程(这里不讲解Gradle 的使用，如果有疑惑可以看官网的 [指南](http://tools.android.com/tech-docs/new-build-system/user-guide))：

## 利用 Android Gradle 的 ProductFlavor 功能添加多个渠道

```
//渠道
    productFlavors {
        wandoujia{
            //这里可以写一些更详细的配置，如果不需要的话，可以忽略
        }
        
        appchina{
        
        }
    }
```

## 在处理 AndroidManifest.xml 文件时添加一个钩子(hook)替换渠道

注意，这里面这是做了一个简单的字符串替换，需要在原来的 `AndroidManifest.xml` 文件中添加：

```
<meta-data android:value="UMENG_CHANNEL_VALUE" android:name="UMENG_CHANNEL"/>
```

这样脚本会替换 `UMENG_CHANNEL_VALUE` 为当前 ProductFlavor 的名称。

```
android.applicationVariants.all{ variant -> 
	println "${variant.productFlavors[0].name}"
    variant.processManifest.doLast{
        copy{
            from("${buildDir}/manifests"){
                include "${variant.dirName}/AndroidManifest.xml"
            }
            into("${buildDir}/manifests/$variant.name")

            filter{
                String line -> line.replaceAll("UMENG_CHANNEL_VALUE", "${variant.productFlavors[0].name}")
            }

            variant.processResources.manifestFile = file("${buildDir}/manifests/${variant.name}/${variant.dirName}/AndroidManifest.xml")
        }    
   }
}
```

###关于Gradle v1.11版本的问题 (Update by <a href="http://feelyou.info" target="_blank" >Remex Huang</a>)

上面的方法在Gradle v1.11版本是无法正常运行的，需要进行一些修改。

```
//替换AndroidManifest.xml的UMENG_CHANNEL_VALUE字符串为渠道名称 By Remex Huang
android.applicationVariants.all{ variant -> 
    variant.processManifest.doLast{
    
        //之前这里用的copy{}，我换成了文件操作，这样可以在v1.11版本正常运行，并保持文件夹整洁
        //${buildDir}是指build文件夹
        //${variant.dirName}是flavor/buildtype，例如GooglePlay/release，运行时会自动生成
        //下面的路径是类似这样：build/manifests/GooglePlay/release/AndroidManifest.xml
        def manifestFile = "${buildDir}/manifests/${variant.dirName}/AndroidManifest.xml"
        
        //将字符串UMENG_CHANNEL_VALUE替换成flavor的名字
        def updatedContent = new File(manifestFile).getText('UTF-8').replaceAll("UMENG_CHANNEL_VALUE", "${variant.productFlavors[0].name}")
        new File(manifestFile).write(updatedContent, 'UTF-8')
        
        //将此次flavor的AndroidManifest.xml文件指定为我们修改过的这个文件
        variant.processResources.manifestFile = file("${buildDir}/manifests/${variant.dirName}/AndroidManifest.xml")
    }    
}
```

更多v1.11的问题，请参考`for1.11_build.gradle`