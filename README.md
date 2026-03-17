# ADR Writer Agent

An AI-powered agent that transforms unstructured meeting notes into formal **Architecture Decision Records (ADRs)** in Markdown. Built with **.NET Aspire**, **Blazor**, **ASP.NET Core**, and the **Microsoft Agent Framework** backed by **Azure OpenAI**.

## Use Case

Maintaining Architecture Decision Records is crucial for any growing microservices ecosystem. ADRs capture the *why* behind technical choices — like why you chose Azure Service Bus over RabbitMQ, or why you opted for an event-driven architecture.

Instead of writing these from scratch, the ADR Writer Agent automates the process:

1. A user logs into the **Blazor Web App**.
2. They provide a messy, unstructured summary of a recent technical sync (e.g., *"We decided to use Redis for distributed caching because our database is getting hammered by read queries, though it adds some infrastructure overhead for the ops team."*).
3. The AI Agent takes this prompt, structures it into a formal Markdown ADR using a standardized template, and returns the polished document.

## Architecture

```
User ──▶ Blazor Web App ──▶ ASP.NET Core Web API ──▶ Azure OpenAI
                                (Agent Framework)
```

| Step | Component | Description |
|------|-----------|-------------|
| 1 | **Blazor Web App** | User inputs raw meeting notes through the UI. |
| 2 | **HTTP POST** | The frontend packages the input as a prompt and sends it to the backend API. |
| 3 | **Web API (Orchestrator)** | ASP.NET Core Web API receives the request, initializes the Microsoft Agent Framework, equips it with custom ADR templating tools, and manages execution. |
| 4 | **Azure OpenAI (Brain)** | The API communicates with Azure OpenAI (e.g., `gpt-4o-mini`) for reasoning and natural language processing. |
| 5 | **Response** | The agent formats the output into a standardized ADR Markdown document and returns it to the Blazor app for review. |

The entire flow is orchestrated using **.NET Aspire**, providing service discovery and built-in telemetry out of the box.

## Solution Structure

```
src/
├── ADRWriter.AppHost/            # .NET Aspire app host — orchestrates all services
│   └── AppHost.cs
├── ADRWriter.ApiService/         # ASP.NET Core Web API — agent orchestration layer
│   ├── Endpoints/
│   │   └── ADRWriterEndpoints.cs
│   ├── Services/
│   │   └── ADRWriterService.cs
│   └── Program.cs
├── ADRWriter.Web/                # Blazor Web App — user-facing frontend
│   ├── ApiClients/
│   │   └── ADRWriterApiClient.cs
│   ├── Components/
│   │   ├── Layout/
│   │   │   ├── MainLayout.razor
│   │   │   └── NavMenu.razor
│   │   └── Pages/
│   │       ├── AdrWriterPage.razor
│   │       ├── Home.razor
│   │       └── Error.razor
│   └── Program.cs
└── ADRWriter.ServiceDefaults/    # Shared Aspire service defaults (resilience, telemetry)
    └── Extensions.cs
```

## Tech Stack

| Layer | Technology |
|-------|------------|
| Frontend | Blazor Web App (.NET 10) |
| Backend API | ASP.NET Core Web API (.NET 10) |
| AI / Agent | Microsoft Agent Framework, Azure OpenAI (`Azure.AI.OpenAI`) |
| Orchestration | .NET Aspire |
| Markdown Rendering | Markdig |
| Observability | OpenTelemetry (OTLP, ASP.NET Core, HTTP, Runtime instrumentation) |
| Resilience | `Microsoft.Extensions.Http.Resilience` |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [.NET Aspire workload](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling)
- An **Azure OpenAI** resource with a deployed model (e.g., `gpt-4o-mini`)

## Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/<your-org>/ADRWriterAgent.git
   cd ADRWriterAgent
   ```

2. **Configure Azure OpenAI credentials**

   Set the required connection details via user secrets in the `ADRWriter.AppHost` project:

   ```bash
   cd src/ADRWriter.AppHost
   dotnet user-secrets set "Azure:OpenAI:Endpoint" "https://<your-resource>.openai.azure.com/"
   dotnet user-secrets set "Azure:OpenAI:ApiKey" "<your-api-key>"
   ```

3. **Run with Aspire**

   ```bash
   cd src/ADRWriter.AppHost
   dotnet run
   ```

   The Aspire dashboard will open in your browser, showing all running services and their endpoints.

## License

This project is provided as-is for educational and demonstration purposes.
