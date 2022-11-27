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
                    { text: 'Introduction', link: '/introduction/intro' },
                    { text: 'Getting Started', link: '/introduction/getting-started' }
                ]
            },
            {
                text: 'REST',
                collapsible: true,
                items: [
                    { text: 'Basics', link: '/rest/basic' },
                    { text: 'Request with body', link: '/rest/body' },
                    { text: 'Authorization', link: '/rest/auth' },
                    { text: 'Testing response', link: '/rest/response' }
                ]
            },
            {
                text: 'SignalR',
                collapsible: true,
                items: [
                    { text: 'Basics', link: '/signalr/basic' }
                ]
            },
            {
                text: 'gRPC',
                collapsible: true,
                items: [
                    { text: 'Basics', link: '/grpc/basic' }
                ]
            },
            {
                text: 'Best practices ',
                collapsible: true,
                items: [
                    { text: 'Extension methods', link: '/best/ext_methods' },
                    { text: 'Test Data', link: '/best/test_data' },
                    { text: 'Infrastructure setting', link: '/best/infrastructure' }
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