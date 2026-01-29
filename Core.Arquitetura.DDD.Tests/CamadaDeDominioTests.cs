using System.Reflection;
using NetArchTest.Rules;

namespace Core.Arquitetura.DDD.Tests;

public class CamadaDeDominioTests
{
    
    [Test]
    public void DominioNaoDeveReferenciarOutrasCamadas()
    {
        
        var assemblies = AssemblyCamadaLoader.Carregar("Core.Dominio.");
        
        Assert.That(assemblies, Is.Not.Empty, "Nenhum assembly de domínio foi carregado.");

        foreach (var assembly in assemblies)
        {
            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(
                    "Core.Aplicacao",
                    "Core.Servico",
                    "Infraestrutura",
                    "Core.Api")
                .GetResult();

            Assert.That(
                result.IsSuccessful,
                Is.True,
                $"Violação arquitetural em {assembly.GetName().Name}"
            );
        }
    }
}