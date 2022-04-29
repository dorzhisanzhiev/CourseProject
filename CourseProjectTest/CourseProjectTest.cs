using CourseProject.Vigenere;
using CourseProjectAsp;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace CourseProjectTest
{
    [TestClass]
    public class CourseProjectTest
    {
        [TestMethod]
        public void VigenereTest_Encrypt()
        {
            string expected = "����������, �� ������� �������� �����!!!";
            string actual = Vigenere.Encrypt("����������, �� ������� �������� �����!!!", "��������");
            Assert.AreEqual(expected, actual);
            expected = "�9���������, �� ������� �������� �����!!!";
            actual = Vigenere.Encrypt("�9���������, �� ������� �������� �����!!!", "�9�������");
            Assert.AreEqual(expected, actual);
            expected = "�9���������, �� ������� �������� \n�����!!!";
            actual = Vigenere.Encrypt("�9���������, �� ������� �������� \n�����!!!", "�9�������");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void VigenereTest_Decrypt()
        {
            string expected = "����������, �� ������� �������� �����!!!";
            string actual = Vigenere.Decrypt("����������, �� ������� �������� �����!!!", "��������");
            Assert.AreEqual(expected, actual);
            expected = "�9���������, �� ������� �������� �����!!!";
            actual = Vigenere.Decrypt("�9���������, �� ������� �������� �����!!!", "�9�������");
            Assert.AreEqual(expected, actual);
            expected = "�9���������, �� ������� �������� \n�����!!!";
            actual = Vigenere.Decrypt("�9���������, �� ������� �������� \n�����!!!", "�9�������");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TxTFileTest_Decrypt()
        {
            string fileName = @".\testFiles\Result_v5.txt"; //��� � ����� ������, ���� ��������
            StringBuilder result = new StringBuilder();
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            string actualInput = result.ToString();
            result = new StringBuilder();
            fileName = @".\testFiles\Result_v5_DecryptionResult.txt";
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            string actualOutput = result.ToString();

            string actual = Vigenere.Decrypt(actualInput, "��������");
            string expected = actualOutput;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void DocxFileTest_Decrypt()
        {
            string fileName = @".\testFiles\Result_v5.docx"; //��� � ����� ������, ���� ��������
            string docText = string.Empty;
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileName, false))
            {
                foreach (Paragraph p in wordDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>())
                {
                    docText += p.InnerText + "\n";
                }
            }
            string actualInput = docText;

            fileName = @".\testFiles\Result_v5_DecryptionResult.docx";
            docText = string.Empty;
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileName, false))
            {
                foreach (Paragraph p in wordDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>())
                {
                    docText += p.InnerText + "\n";
                }
            }
            string actualOutput = docText;

            string actual = Vigenere.Decrypt(actualInput, "��������");
            string expected = actualOutput;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TxTFileTest_Save()
        {
            string fileDirectory = @".\testFiles\";
            string fileName = "SaveResult";
            string extension = ".txt";
            fileName += extension;
            var fileUploadPath = Path.Combine(fileDirectory, fileName);
            string output = "����������, �� ������� �������� �����!!! ";
            try
            {
                using (var fileStream = new FileStream(fileUploadPath, FileMode.Create))
                {
                }
                File.WriteAllText(fileUploadPath, output);
            }
            catch
            {
            }
            StringBuilder result = new StringBuilder();
            using (StreamReader reader = new StreamReader(fileUploadPath))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            string actual = result.ToString().TrimEnd('\r', '\n');
            string expected = output;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void DocxFileTest_Save()
        {
            string fileDirectory = @".\testFiles\";
            string fileName = "SaveResult";
            string extension = ".docx";
            fileName += extension;
            var fileUploadPath = Path.Combine(fileDirectory, fileName);
            string output = "����������, �� ������� �������� �����!!! ";
            try
            {
                string[] outputLines = output.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(fileUploadPath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    foreach (string line in outputLines)
                    {
                        Paragraph para = body.AppendChild(new Paragraph());
                        Run run = para.AppendChild(new Run());
                        run.AppendChild(new Text(line));
                    }
                }
            }
            catch
            {
            }
            string docText = string.Empty;
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileUploadPath, false))
            {
                foreach (Paragraph p in wordDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>())
                {
                    docText += p.InnerText + "\n";
                }
            }
            string actual = docText.TrimEnd('\r', '\n');
            string expected = output;
            Assert.AreEqual(expected, actual);
        }
    }
}