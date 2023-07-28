# GrpcVideoStreaming
Sample Grpc streaming example.

Below the actors:

- **StreamingHub**: the service to handle sender and receivers
- **StreamingSender**: the *Console Application* reads *.jpg* files and send them to the *StreamingHub*
- **SttreamingViewer**: the *Blazor WebAssembly* application shows the streaming coming from the *StreamingHub*
