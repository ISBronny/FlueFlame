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
                    { text: 'Getting Started', link: '/grpc/basic' },
                    { text: 'Configuring gRPC', link: '/grpc/basic' },
                    { text: 'Unary RPC', link: '/grpc/basic' },
                    { text: 'Server streaming RPC', link: '/grpc/basic' },
                    { text: 'Client streaming RPC', link: '/grpc/basic' },
                    { text: 'Bidirectional streaming RPC', link: '/grpc/basic' },
                    { text: 'RPC Errors', link: '/grpc/basic' },
                    { text: 'Authorization', link: '/grpc/basic' },
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