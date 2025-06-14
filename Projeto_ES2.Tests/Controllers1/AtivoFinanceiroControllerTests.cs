using NUnit.Framework;
using Projeto_ES2.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Server.Data;
using System;
using System.Threading.Tasks;
using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Tests.Controllers1
{
    public class AtivoFinanceiroControllerTests
    {
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
        }

        [Test]
        public async Task GetAtivoFinanceiro_RetornaNotFound_ParaIdInvalido()
        {
            // Arrange
            var controller = new AtivoFinanceiroController(_context);

            // Act
            var resultado = await controller.GetAtivoFinanceiro(Guid.NewGuid());

            // Assert
            Assert.That(resultado.Result, Is.InstanceOf<NotFoundResult>());
        }
        
        [Test]
        public async Task GetAtivoFinanceiro_DeveRetornarOk_QuandoAtivoExiste()
        {
            // Arrange
            var ativoId = Guid.NewGuid();
            var ativo = new AtivoFinanceiro
            {
                Id = ativoId,
                Nome = "Ativo de Teste",
                Tipo = TipoAtivoFinanceiro.FundoInvestimento,
                DataInicio = DateTime.Now
            };

            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            var controller = new AtivoFinanceiroController(_context);

            // Act
            var resultado = await controller.GetAtivoFinanceiro(ativoId);

            // Assert
            Assert.That(resultado.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = resultado.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.InstanceOf<AtivoFinanceiro>());

            var ativoResultado = (AtivoFinanceiro)okResult.Value!;
            Assert.That(ativoResultado.Id, Is.EqualTo(ativoId));
        }


    }
}