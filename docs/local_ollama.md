# Using a local LLM server

You may want to save costs by developing against a local LLM server, such as[Ollama](https://www.ollama.com). Note that a local LLM will generally be slower and not as sophisticated.

Once you've got your local LLM running and serving an OpenAI-compatible endpoint, define `LOCAL_ENDPOINT` in your `appsettings.local.json` file.

For example, to point at a local Ollama server running on its default port, using the Phi3.5 model, modify `appsettings.local.json` to:

```JSON
"LOCAL_MODEL_NAME": "phi3.5",
"LOCAL_ENDPOINT": "http://localhost:11434"
```

If you're running inside a dev container, use this local URL instead:

```JSON
LOCAL_ENDPOINT="http://host.docker.internal:11434/v1"
```
