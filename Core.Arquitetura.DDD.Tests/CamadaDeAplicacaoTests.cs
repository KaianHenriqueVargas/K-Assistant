using NetArchTest.Rules;

namespace Core.Arquitetura.DDD.Tests;

public class CamadaDeAplicacaoTests
{
    [Test]
    public void AplicacaoSoDeveDependenderDeDominio()
    {
        var assemblies = AssemblyCamadaLoader.Carregar("Core.Api.Aplicacao.");

        Assert.That(assemblies, Is.Not.Empty,
            "Nenhum assembly de aplicacao foi carregado.");

        foreach (var assembly in assemblies)
        {
            var result = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(
                    "Core.Api.Servico",
                    "Infraestrutura",
                    "Core.Api")
                .GetResult();

            Assert.That(result.IsSuccessful, Is.True,
                $"Violação no assembly {assembly.GetName().Name}");
        }
    }
}