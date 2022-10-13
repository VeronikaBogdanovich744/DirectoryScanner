using DirectoryScannerLibrary;
using DirectoryScannerLibrary.Models;
using System.DirectoryServices.ActiveDirectory;

namespace DirectoryTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestNumberOfFilesInDirectory()
        {
            var directoryTracer = new DirectoryTracer();
            directoryTracer.traceMainDirectory("C:\\Users\\Veronika\\Documents\\work\\5 сем\\ОСиСП\\OSaSP2_2");
            while (directoryTracer.queue.IsWorking != 3) ;

            Assert.AreEqual(directoryTracer.Files.Count, 1); //первая вложенность
            DirectoryScannerLibrary.Models.File file;
            directoryTracer.Files.TryGetValue("C:\\Users\\Veronika\\Documents\\work\\5 сем\\ОСиСП\\OSaSP2_2",out file);
            
            Assert.IsNotNull(file);
            Assert.AreEqual(file.Files.Count, 4);

        }

        [TestMethod]
        public void TestNumberOfFilesInDirectory2()
        {
            var directoryTracer = new DirectoryTracer();
            directoryTracer.traceMainDirectory("C:\\Users\\Veronika\\Downloads\\MO_bsuir-main");
            while (directoryTracer.queue.IsWorking != 3) ;

            Assert.AreEqual(directoryTracer.Files.Count, 1); //первая вложенность
            DirectoryScannerLibrary.Models.File file;
            directoryTracer.Files.TryGetValue("C:\\Users\\Veronika\\Downloads\\MO_bsuir-main", out file);

            Assert.IsNotNull(file);
            Assert.AreEqual(file.Files.Count, 2);

            file.Files.TryGetValue("C:\\Users\\Veronika\\Downloads\\MO_bsuir-main\\МО", out file);

            Assert.IsNotNull(file);
            Assert.AreEqual(file.Files.Count, 4);

            file.Files.TryGetValue("C:\\Users\\Veronika\\Downloads\\MO_bsuir-main\\МО\\lab1", out file);

            Assert.IsNotNull(file);
            Assert.AreEqual(file.Files.Count, 2);

        }

        [TestMethod]
        public void TestNumberOfFilesInDirectory3()
        {
            var directoryTracer = new DirectoryTracer();
            directoryTracer.traceMainDirectory("C:\\Users\\Veronika\\Desktop");
            Thread.Sleep(6000);

            Assert.AreEqual(directoryTracer.Files.Count, 1); //первая вложенность
            DirectoryScannerLibrary.Models.File file;
            directoryTracer.Files.TryGetValue("C:\\Users\\Veronika\\Desktop", out file);

            Assert.IsNotNull(file);
            Assert.AreEqual(file.Files.Count, 5);

            file.Files.TryGetValue("C:\\Users\\Veronika\\Desktop\\LR3", out file);

            Assert.IsNotNull(file);
            Assert.AreEqual(file.Files.Count, 1);

        }
    }
}