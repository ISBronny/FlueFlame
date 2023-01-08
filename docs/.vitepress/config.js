export default {
    lang: 'en-US',
    base: '/FlueFlame/',
    title: 'FlueFlame',
    description: 'Fluent testing',

    lastUpdated: true,
    cleanUrls: 'without-subfolders',

    themeConfig: {
        logo: '/FlueFlameLogo.svg',
        sidebar: [
            {
                text: 'Introduction',
                collapsible: true,
                items: [
                    { text: 'What is it?', link: '/introduction/what-is-it' },
                    { text: 'Getting Started', link: '/introduction/getting-started' },
                    { text: 'Architecture', link: '/introduction/architecture' },
                ]
            },
            {
                text: 'REST',
                collapsible: true,
                items: [
                    { text: 'Getting Started', link: '/rest/getting-started' },
                    { text: 'Configuration', link: '/rest/configuration' },
                    { text: 'ASP.NET Core Integration', link: '/rest/asp-net' },
                    { text: 'Sending requests', link: '/rest/send' },
                    { text: 'Authorization', link: '/rest/auth' },
                    { text: 'Testing response', link: '/rest/response' }
                ]
            },
            {
                text: 'gRPC',
                collapsible: true,
                items: [
                    { text: 'Getting Started', link: '/grpc/getting-started' },
                    { text: 'Configuring gRPC', link: '/grpc/configuration' },
                    { text: 'Unary RPC', link: '/grpc/unary' },
                    { text: 'Server streaming RPC', link: '/grpc/server-stream' },
                    { text: 'Client streaming RPC', link: '/grpc/client-stream' },
                    { text: 'Bidirectional streaming RPC', link: '/grpc/bidirectional' },
                    { text: 'Low-level access', link: '/grpc/raw' },
                    { text: 'Authorization', link: '/grpc/auth' },
                ]
            },
            {
                text: 'SignalR',
                collapsible: true,
                items: [
                    { text: 'Getting Started', link: '/signalr/basic' }
                ]
            }
        ],
        socialLinks: [
            { icon: 'github', link: 'https://github.com/ISBronny/FlueFlame' }
        ],
        editLink: {
            pattern: 'https://github.com/ISBronny/FlueFlame/edit/master/docs/:path'
        }
    }
}