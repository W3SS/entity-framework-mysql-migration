## Issue

- Test Runner ของ Xamarin Studio อ่าน Config ไฟล์ผิดที่
- ถ้าแสดงไฟล์ Config ปัจจุบันจะได้ Config อยู่ที่
- `/Applications/Xamarin Studio.app/Contents/Resources/lib/monodevelop/AddIns/MonoDevelop.UnitTesting/NUnit2/EFMigration.Tests.dll.config`  
- แทนที่จะเป็น `./EFMigration.Tests/bin/Debug/EFMigration.Tests.dll.config`


### โค้ดที่ใช้แสดง Path ของไฟล์ Config

```csharp
[Test]
public void ShouldGetConfigFile2 ()
{
    var config = ConfigurationManager.OpenExeConfiguration (ConfigurationUserLevel.None);
    Assert.Fail (config.FilePath);
}
```

- ปัญหานี้จะไม่เจอเมื่อรันโปรแกรมแบบ ConsoleApplication หรือรันด้วย Test runner ของ VisualStudio
- คาดว่าน่าจะเป็น Bug test runner ของ Xamarin Studio

