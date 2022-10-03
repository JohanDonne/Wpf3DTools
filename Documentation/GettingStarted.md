### Getting Started

Using the Wpf3dTools library is easy:

* Add the Wpf3dTools [Nuget package](https://www.nuget.org/packages/Wpf3dTools) to your project. 
* For Class libraries: Make sure you create a WPF Class Library (at least .Net 6). For existing libraries you can edit the '.csproj' file: set the TargetFramework to (at least) 'net6.0-windows' and include the `<UseWPF>true</UseWPF>` property:    
  
```<language>
  <PropertyGroup>
    <TargetFramework>net6.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>
```


* Start using the Wpf3dTools classes...