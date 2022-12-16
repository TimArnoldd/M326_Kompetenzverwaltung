using BL.Data;
using BL.Enums;
using BL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BL.Tests
{
    [TestClass()]
    public class BTests
    {
        private B GetB()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CompetenceManagement")
                .Options;

            var context = new ApplicationDbContext(options);
            return new B(context);
        }


        #region Create
        [TestMethod()]
        public void CreateCompetenceArea_ShouldWork_WithValidArea()
        {
            // Arrange
            B b = GetB();
            var area = new CompetenceArea { Name = "Konis Hupen" };

            // Act
            b.CreateCompetenceArea(area);

            // Assert
            Assert.AreEqual(1, b.GetAllAreas().Count());
        }
        [TestMethod()]
        public void CreateCompetenceArea_ShouldNotWork_WithInvalidArea()
        {
            // Arrange
            B b = GetB();
            var area = new CompetenceArea { Name = "" };

            // Act
            b.CreateCompetenceArea(area);
            b.CreateCompetenceArea(null!);

            // Assert
            Assert.AreEqual(0, b.GetAllAreas().Count());
        }

        [TestMethod()]
        public void CreateCompetence_ShouldWork_WithValidCompetence()
        {
            // Arrange
            B b = GetB();
            var competence = new Competence { Name = "Konis Hupen", Description = "Extrem", CompetenceArea = new() { Id = 1 } };

            // Act
            b.CreateCompetence(competence);

            // Assert
            Assert.AreEqual(1, b.GetAllCompetences().Count());
        }
        [TestMethod()]
        public void CreateCompetence_ShouldNotWork_WithInvalidCompetence()
        {
            // Arrange
            B b = GetB();
            var competences = new List<Competence>
            {
                new() { Name = "", Description = "Extrem", CompetenceArea = new() { Id = 1 } },
                new() { Name = "Konis Hupen", Description = "", CompetenceArea = new() { Id = 1 } },
                new() { Name = "Konis Hupen", Description = "Extrem", CompetenceArea = new() { Id = 0 } },
                new() { Name = "Konis Hupen", Description = "Extrem", CompetenceArea = new() { Id = -123 } }
            };

            // Act
            foreach (var competence in competences)
            {
                b.CreateCompetence(competence);
            }
            b.CreateCompetence(null!);

            // Assert
            Assert.AreEqual(0, b.GetAllCompetences().Count());
        }

        [TestMethod()]
        public void CreateResource_ShouldWork_WithValidResource()
        {
            // Arrange
            B b = GetB();
            var resource = new Resource { DisplayText = "Mandelbulber", Link = "https://mandelbulber.ch", Competence = new() { Id = 1 } };

            // Act
            b.CreateResource(resource);

            // Assert
            Assert.AreEqual(1, b.GetAllResources().Count());
        }
        [TestMethod()]
        public void CreateResource_ShouldNotWork_WithInvalidResource()
        {
            // Arrange
            B b = GetB();
            var resources = new List<Resource>
            {
                new() { DisplayText = "", Link = "https://mandelbulber.ch", Competence = new() { Id = 1 } },
                new() { DisplayText = "Mandelbulber", Link = "", Competence = new() { Id = 1 } },
                new() { DisplayText = "Mandelbulber", Link = "Kein gültiger Link", Competence = new() { Id = 1 } },
                new() { DisplayText = "Mandelbulber", Link = "https://mandelbulber.ch", Competence = new() { Id = 0 } },
                new() { DisplayText = "Mandelbulber", Link = "https://mandelbulber.ch", Competence = new() { Id = -123 } }
            };

            // Act
            foreach (var resource in resources)
            {
                b.CreateResource(resource);
            }
            b.CreateResource(null!);

            // Assert
            Assert.AreEqual(0, b.GetAllResources().Count());
        }
        #endregion

        #region Read
        [TestMethod()]
        public void GetCompetence_ShouldReturnCompetence_WithValidId()
        {
            // Arrange
            B b = GetB();
            var competence = new Competence() { Name = "Konis Hupen", Description = "Extrem", Level = CompetenceLevel.Hard, CompetenceArea = new() { Id = 1 } };
            b.CreateCompetence(competence);

            // Act
            var dbCompetence = b.GetCompetence(1);

            // Assert
            Assert.IsNotNull(dbCompetence);
            Assert.AreEqual(competence.Name, dbCompetence.Name);
            Assert.AreEqual(competence.Description, dbCompetence.Description);
            Assert.AreEqual(competence.Level, dbCompetence.Level);
        }
        [TestMethod()]
        public void GetCompetence_ShouldReturnNull_WithInvalidId()
        {
            // Arrange
            B b = GetB();

            // Act
            var dbCompetence = b.GetCompetence(123); // Should not throw Exception

            // Assert
            Assert.IsNull(dbCompetence);
        }
        #endregion
    }
}