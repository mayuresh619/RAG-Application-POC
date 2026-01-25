# 🤖 Local-Doc AI: Ollama & Kernel Memory RAG

A powerful, local-first **Retrieval-Augmented Generation (RAG)** console application. This project uses **Microsoft Kernel Memory** to bridge the gap between your local PDF documents and **Ollama**, allowing you to chat with your data without it ever leaving your machine.



---

## ✨ Features

* **Dual-Mode Operation:** * **Mode 1 (Document RAG):** Queries your specific PDF for facts and localized information.
    * **Mode 2 (Direct LLM):** Standard chat mode for general brainstorming and reasoning.
* **Fully Local:** Leverages Ollama for both text generation and vector embeddings.
* **Context-Aware:** Uses the `nomic-embed-text` model to create high-dimensional embeddings for accurate document retrieval.
* **High Performance:** Optimized with `gemma3:1b` for fast, efficient local inference.

---

## 🛠️ Prerequisites

Before running the application, ensure you have the following set up:

1.  **Ollama Installed:** [Download Ollama](https://ollama.ai/)
2.  **Required Models:**
    ```bash
    ollama pull gemma3:1b
    ollama pull nomic-embed-text:latest
    ```
3.  **.NET 8.0 SDK** or later.

---

## 🚀 Getting Started

### 1. Setup the Environment
Clone this repository and ensure your local Ollama server is running (usually at `http://localhost:11434`).

### 2. Add Your Document
Place the PDF you want to analyze in the project
