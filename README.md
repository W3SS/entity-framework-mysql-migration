## Code First Migrations (Visual Studio)

#### `Enable-Migrations –EnableAutomaticMigrations`

```csharp
public class Customer {
    [Key]
    public int Id { set; get; }
    public string FirstName { set; get; }
    public string LastName { set; get; }
    public int Age { set; get; }
    public int Telephone { set; get; }
}

public class EFContext : DbContext {
    public EFContext() { }
    public EFContext (string connName):base (connName) { }
    public DbSet<Customer> Customers { set; get; }
}

```

- เป็นคำสั่ง Enable migration โดยจะพิมพ์คำสั่งนี้ใน `Package manager console`
- Context จะต้องมี Default constructor เพื่อให้โปรแกรมสามารถเข้าไปอ่าน Property
- ถ้าต้องการ Migration โดยเริ่มจากฐานข้อมูลที่มี Table อยู่แล้ว ให้เพิ่ม ` –IgnoreChange` เพื่อ Ignore Creation Script
- ในกรณีที่ Context ประกาศไว้ที่ Assembly อื่น สามารถใช้ Flag `-ContextAssemblyName` เพื่อระบบชื่อ `Dll` ได้ เช่น

```
Enable-Migrations -ContextAssemblyName DbEntity -Verbose
```

#### `Add-Migration Initilize -ConnectionStringName mac`

- สร้าง Migration ครั้งแรกโปรแกรมจะ Gen code สำหรับ Create table

#### `Update-Database -ConnectionStringName mac`

- Update ฐานข้อมูล
- ครั้งแรกจะมี Error `No MigrationSqlGenerator`
- `No MigrationSqlGenerator found for provider 'MySql.Data.MySqlClient'. Use the SetSqlGenerator method in the target migrations configuration class to register additional SQL generators.`
- ต้องเพิ่มคำสั่ง `SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator())` ในไฟล์ `Migrations\Configration.cs`
- รันคำสั่งใหม่อีกครั้ง

#### `Add-Migration AddAgeAndTelephone -ConnectionStringName mac`

- พิมพ์คำสั่งนี้หลังจากแก้ไข Entity class เช่น เพิ่ม Field Age ใน Table Customer

```csharp
public class Customer {
    [Key]
    public int Id { set; get; }
    public string FirstName { set; get; }
    public string LastName { set; get; }
    public int Age { set; get; }
    public int Telephone { set; get; }
}
```

#### `Update-Database -ConnectionStringName mac`

- Update ฐานข้อมูลอีกครั้ง

#### `Update-Database -Script -SourceMigration: Initial -TargetMigration: AddAgeAndTelephone -ConnectionStringName mac`

- Gen sql script เฉพาะส่วนที่มีการ Update

```sql
alter table `Customers` add column `Age` int not null  
alter table `Customers` add column `Telephone` int not null  
alter table `Customers` modify `FirstName` longtext
alter table `Customers` modify `LastName` longtext
```

## Issues

#### คำสั่ง `Enable-Migrations` ไม่มี `-ContextAssemblyName`

`Help Enable-Migrations` ไม่มี Option `-ContextAssemblyName`

```powershell
SYNTAX
    Enable-Migrations [-ContextTypeName <String>] [-EnableAutomaticMigrations] [-MigrationsDirectory <String>] [-ProjectName <String>] [-StartUpProjectName <String>]
    [-ContextProjectName <String>] [-ConnectionStringName <String>] [-Force] [<CommonParameters>]

    Enable-Migrations [-ContextTypeName <String>] [-EnableAutomaticMigrations] [-MigrationsDirectory <String>] [-ProjectName <String>] [-StartUpProjectName <String>]
    [-ContextProjectName <String>] -ConnectionString <String> -ConnectionProviderName <String> [-Force] [<CommonParameters>]
```

ไม่สามารถระบุชื่อ `Assembly` ได้

```powershell
Enable-Migrations -ContextAssemblyName DbEntity -ProjectName DbEntity.Migrations -Verbose
Enable-Migrations : A parameter cannot be found that matches parameter name 'ContextAssemblyName'.
At line:1 char:19
+ Enable-Migrations -ContextAssemblyName DbEntity -ProjectName DbEntity ...
+                   ~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : InvalidArgument: (:) [Enable-Migrations], ParameterBindingException
    + FullyQualifiedErrorId : NamedParameterNotFound,Enable-Migrations
```

ปกติ `Help Enable-Migrations` ต้องได้

```powershell
SYNTAX
    Enable-Migrations [-ContextTypeName <String>] [-EnableAutomaticMigrations] [-MigrationsDirectory <String>] [-ProjectName <String>] [-StartUpProjectName <String>]
    [-ContextProjectName <String>] [-ConnectionStringName <String>] [-Force] [-ContextAssemblyName <String>] [-AppDomainBaseDirectory <String>] [<CommonParameters>]

    Enable-Migrations [-ContextTypeName <String>] [-EnableAutomaticMigrations] [-MigrationsDirectory <String>] [-ProjectName <String>] [-StartUpProjectName <String>]
    [-ContextProjectName <String>] -ConnectionString <String> -ConnectionProviderName <String> [-Force] [-ContextAssemblyName <String>] [-AppDomainBaseDirectory
    <String>] [<CommonParameters>]
```

แก้ไข

- ยังไม่ทราบสาเหตุ แก้ไขเบื้องต้นโดยการสร้าง Solution ว่าง ๆ แล้ว Import Project เข้าไปใหม่


#### `Invalid for use as a key column in an index`

ไม่สามารถเพิ่ม Index ไปยัง Field ที่ต้องการเนื่องจาก Type ของ Field นั้นไม่ถูกต้อง

```
Column 'Guid' in table 'dbo.DbFileInfoes' is of a type that is invalid for use as a key column in an index.
```

```fsharp
[<Index>]
member val Guid = String.Empty with get, set
```

แก้ไขโดยเพิ่ม `StringLength`

```fsharp
[<Index>]
[<StringLength(50)>]
member val Guid = String.Empty with get, set
```

#### กรณีรัน Unit test บน Xamarin Studio

- Test Runner ของ Xamarin Studio อ่าน Config ไฟล์ผิดที่
- ถ้าแสดงไฟล์ Config ปัจจุบันจะได้ Config อยู่ที่
- `/Applications/Xamarin Studio.app/Contents/Resources/lib/monodevelop/AddIns/MonoDevelop.UnitTesting/NUnit2/EFMigration.Tests.dll.config`  
- แทนที่จะเป็น `./EFMigration.Tests/bin/Debug/EFMigration.Tests.dll.config`

โค้ดที่ใช้แสดง Path ของไฟล์ Config

```csharp
[Test]
public void ShouldGetConfigFile2 () {
    var config = ConfigurationManager.OpenExeConfiguration (ConfigurationUserLevel.None);
    Assert.Fail (config.FilePath);
}
```

- ปัญหานี้จะไม่เจอเมื่อรันโปรแกรมแบบ ConsoleApplication หรือรันด้วย Test runner ของ VisualStudio
- คาดว่าน่าจะเป็น Bug test runner ของ Xamarin Studio

แก้ไข

- แก้เบื้องต้นโดย Test ผ่าน Command line

> nunit-console2 EFMigration.Tests/bin/Debug/EFMigration.Tests.dll

## Link

- https://corengen.wordpress.com/2010/01/22/nunit-and-application-configuration-files
- https://msdn.microsoft.com/en-us/data/jj591621.aspx
- https://romiller.com/2012/02/09/running-scripting-migrations-from-code/
- https://msdn.microsoft.com/en-us/data/jj591621
- https://channel9.msdn.com/Blogs/EF/Migrations-Existing-Databases
- http://webandlife.blogspot.com/2015/07/code-first-migrations-with-entity.html
- http://www.thinkprogramming.co.uk/post/code-first-with-mysql-and-entity-framework-6