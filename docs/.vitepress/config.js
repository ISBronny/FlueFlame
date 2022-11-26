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
                    { text: 'Introduction', link: '/introduction/intro' }
                ]
            },
            {
                text: 'REST',
                collapsible: true,
                items: [
                    { text: 'Basics', link: '/rest/basic' }
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