# Acesso aos Fontes

* GitHub: https://github.com/leomar-sartor/BancoDigital

## Baixar, Configurar e Rodar API

Primeiramente devemos ter o fonte em maõs, então baixe-o em uma pasta de sua preferência, conforme imagem a seguir.

![Download Fonte](https://raw.githubusercontent.com/leomar-sartor/BancoDigital/master/Documentos/DownloadFonte.png)

Posterirmente abra-o com o Visual Studio 2019, antes, precisamos ajustar as portas de acessos do projeto, acessando a as propriedades da solução principal com o clique direito do mouse, conforme imagem abaixo.

![Configuração Visual Studio](https://raw.githubusercontent.com/leomar-sartor/BancoDigital/master/Documentos/RodarLocalmenteVisualStudio.png)

Detalhe. O Bando de Dados escolhido foi MySQL, portanto não esqueça de criar seu próprio banco e de trocar a Connetion String de acordo com a configuração de seu banco, localizado no arquivo de configuração 'appsettings.json' para rodar a API e as variáveis localizadas no caminho BancoDigital.Entidades/Contexto/Contexto.cs para a execução dos teste, conforme a seguir.

~~~Json
{
  "ConnectionStrings": {
    "ProductionConnection": "server=localhost;database=bancodigitaldatabase;uid=root;pwd=sua_senha;port=3306",
    "HomologConnection": "server=localhost;database=bancohomologacao;uid=root;pwd=sua_senha;port=3306"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
~~~

Prontinho, API rodando. Agora podemos testar com o PostMan, que pode ser encontrada em 'Documentação e Imagens Auxiliares' ou então integrar alguma aplicação de sua preferência.

# Cenário da API

## Considerações:

* O sistema permite o cadastro de uma ou mais contas por meio de um identificardor string como por exxemplo: '0.12-3' único não permitindo contas com o mesmo identificador, para posteriormente consultarse os saldo pelo número desta conta;
* Considerasse que saques não devem ocorrer quando não houver saldo, como acontece em bancos tradicionais, que concedem um limite;
* Saque e Depositos aceitam apenas valores maior que Zero (Valores Válidos);
* Teste de API com PostMan;

## Documentação e Imagens Auxiliares

O projeto contém uma pasta 'Documentos' na pasta principal, a qual tem imagens que auxiliam e instruem como testar a API e como executar o procedimento de Cobertura de Código, divididos por cada etapa. Você pode acessa-lá clicando a seguir: <https://github.com/leomar-sartor/BancoDigital/tree/master/Documentos>.

Você também pode visualizar o resultado final da Cobertura de Código realizando o download em <https://github.com/leomar-sartor/BancoDigital/tree/master/Documentos/5%20-%20Relat%C3%B3rio%20Cobertura%20de%20C%C3%B3digo/Relatorio(Abrir%20index.html)> e abrindo o arquivo index.html em seu Navegador. Deverá aparece algo como a imagem abaixo.

![Cobertura de Código](https://raw.githubusercontent.com/leomar-sartor/BancoDigital/master/Documentos/5%20-%20Relat%C3%B3rio%20Cobertura%20de%20C%C3%B3digo/CoberturaDetalhada.png)

## Endpoint's

Endpoint   | Verbo | Objetivo | Retornos possíveis  |
---------- | ------|----------|---------------------|
contacorrente/adicionar | POST |Cadastrar uma conta digital |  200 (Conta criada) e 500 (Conta digital inválida) e 400 (Conta sem número)
contacorrente/depositar | POST | Depositar um valor válido   |  200 (Depóstito realizado) e 500 (Valor inválido)
contacorrente/sacar     | POST | Sacar um valor válido       | 200 (Saque realizado) e 500 (Saldo insuficiente ou Valor inválido)
saldo/numeroConta       | GET  | Verificar saldo na conta    |  200 (Saldo consultado) e 404 (Conta não encontrada)

## Url's

 Exemplo de Url Local                                           |   
----------------------------------------------------------------|
 http://localhost:3490/contacorrente/adicionar                  |
 http://localhost:3490/contacorrente/sacar                      |
 http://localhost:3490/contacorrente/depositar                  |
 http://localhost:3490/contacorrente/saldo?numeroConta=24.819-3 |

# Ferramentas de Testes e Cobertura de Código

Utilizou-se neste projeto a ferramenta OpenCover para gerar um XML de cobertura de códgo com base nos teste unitários, em conjunto com ReportGenerator para tornar o relatório compreensível, disponibilizando um arquivo HTML. Procedimentos realizados e passos necessários a seguir:

## Gerando XML com OpenCover para Cobertura de Código 

1. Download OpenCover(.zip): https://github.com/OpenCover/opencover/releases/tag/4.7.1221
2. Adicionado na Pasta Raiz: 'BancoDigital/OpenCoverXml/opencover.4.7.1221/'
3. Acessado caminho raiz pelo PowerShell como Administrador
4. Rodar comando a seguir, que resultará em um Arquivo 'Teste.xml' dentro de BancoDigital/OpenCoverXml.

> .\OpenCover.Console.exe 
> -target:"C:\Program Files\dotnet\dotnet.exe" 
> -targetargs:"test C:\Projetos\BancoDigital\BancoDigital.Testes\BancoDigital.Testes.csproj" 
> -output:"C:\Projetos\BancoDigital\BancoDigital\OpenCoverXML\Teste.xml" 
> -oldStyle	
> -filter:"+[BancoDigital.Testes*]*" 

~~~PowerShell
.\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:"test C:\Projetos\BancoDigital\BancoDigital.Testes\BancoDigital.Testes.csproj" -output:"C:\Projetos\BancoDigital\BancoDigital\OpenCoverXML\Teste.xml" -oldStyle -filter:"+[BancoDigital*]*"
~~~

## Gerando relatório HTML com Report Generator para visualização no Navegador

1. Download Reportgenerator(.zip): https://github.com/danielpalme/ReportGenerator/releases/tag/v4.8.11
2. Adicionado na Pasta Raiz: 'BancoDigital/Reports/ReportGenerator_4.8.11/'
3. Acessado caminho raiz pelo PowerShell como Administrador
4. Rodar comando a seguir, que resultará em diversos arquivos dentro de BancoDigital/Reports.
  
> .\ReportGenerator.exe
> -reports:"C:\Projetos\BancoDigital\BancoDigital\OpenCoverXML\Teste.xml"
> -targetdir:"C:\Projetos\BancoDigital\BancoDigital\Reports"
> -sourcedirs:"C:\Projetos\BancoDigital\BancoDigital\Reports"

~~~PowerShell
.\ReportGenerator.exe -reports:"C:\Projetos\BancoDigital\BancoDigital\OpenCoverXML\Teste.xml" -targetdir:"C:\Projetos\BancoDigital\BancoDigital\Reports" -sourcedirs:"C:\Projetos\BancoDigital\BancoDigital\Reports"
~~~

5. Acessar arquivo 'index.html' e visualizar a cobertura de Código gerada pelos testes unitários.
