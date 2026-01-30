using NetArchTest.Rules;

namespace Core.Arquitetura.DDD.Tests;

[TestFixture]
public class CamadaDeServicoTests
{
    [Test]
    public void ServicoNaoDeveReferenciarInfraOuApi()
    {
        var assemblies = AssemblyCamadaLoader.Carregar("Core.Api.Servico.");

        // Se não houver assemblies, o teste passa automaticamente
        // porque não há nada para testar/violar
        if (assemblies.Count == 0)
        {
            Assert.Pass("Nenhum assembly de serviço encontrado. Teste ignorado.");
            return; // ou apenas Assert.Pass já sai do método
        }

        foreach (var assembly in assemblies)
        {
            var result = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(
                    "Infraestrutura",
                    "Core.Api")
                .GetResult();

            Assert.That(result.IsSuccessful, Is.True,
                $"Violação no assembly {assembly.GetName().Name}");
        }
    }
}