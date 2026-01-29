using NetArchTest.Rules;

namespace Core.Arquitetura.DDD.Tests;

[TestFixture]
public class CamadaDeInfraestruturaTests
{
    [Test]
    public void InfraestruturaNaoDeveReferenciarServicoOuApi()
    {
              
        var assemblies = AssemblyCamadaLoader.Carregar("Infraestrutura");
        
        Assert.That(assemblies, Is.Not.Empty, "Nenhum assembly de infraestrutura foi carregado.");

        foreach (var assembly in assemblies)
        {
            var result = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(
                    "Core.Servico",
                    "Core.Api")
                .GetResult();

            Assert.That(result.IsSuccessful, Is.True,
                $"Violação no assembly {assembly.GetName().Name}");
        }
    }
}