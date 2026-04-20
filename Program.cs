using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory.AI.Ollama;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using System.Text;

var ollamaConfig = new OllamaConfig
{
    Endpoint = "http://localhost:11434",
    TextModel = new OllamaModelConfig("gemma3:1b", 8192),      // Your chat model
    EmbeddingModel = new OllamaModelConfig("nomic-embed-text:latest", 2048) // Your embedding model
};

var postgresConfig = new PostgresConfig
{
    ConnectionString = "Host=localhost;Port=5432;Database=ragdb;Username=postgres;Password=123456789",
    Schema = "public",
};

var chat = new OllamaTextGenerator(ollamaConfig);

var memory = new KernelMemoryBuilder()
    .WithOllamaTextGeneration(ollamaConfig)
    .WithOllamaTextEmbeddingGeneration(ollamaConfig)
    .WithPostgresMemoryDb(postgresConfig)
    .WithSimpleFileStorage(new SimpleFileStorageConfig
    {
        StorageType = FileSystemTypes.Disk,
        Directory = "km-cache"
    })
    .Build();

string docid = await memory.ImportDocumentAsync(Path.GetFullPath("Society_Rules_Sample.pdf"));

while (!await memory.IsDocumentReadyAsync(docid))
{
Console.WriteLine("Still indexing...");
await Task.Delay(1000);
}
Console.WriteLine("Document ready!");

GoHere:
    Console.WriteLine("Please choose mode:");
Console.WriteLine("1. From Document");
Console.WriteLine("2. For LLM");
var mode = Console.ReadLine();
bool useDocs = false;
if(mode == "1")
    useDocs = true;
else if(mode == "2")
    useDocs = false;
else
{
    Console.WriteLine("Please enter valid input");
    goto GoHere;
}

    while (true)
    {

        Console.WriteLine("You: ");
        var Question = Console.ReadLine();

        if (useDocs)
        {
            Console.WriteLine("Using documents...");
            var answer = await memory.AskAsync(Question);
            Console.WriteLine(answer.Result);
        }
        else
        {
            Console.WriteLine("Normal chat...");
            var options = new TextGenerationOptions
            {
                MaxTokens = 500,
                Temperature = 0.7f,
                FrequencyPenalty = 0.5f,
                StopSequences = ["END", "STOP"]
            };
            StringBuilder sb = new StringBuilder();

            await foreach (var chunk in chat.GenerateTextAsync(Question, options))
            {
                sb.Append(chunk.Text);
            }
            string response = sb.ToString();
            Console.WriteLine("AI: " + response);
        }
    }