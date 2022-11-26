export default {
    base: '/FlueFlame/',
    title: 'FlueFlame',
    description: 'Fluent testing',
    themeConfig: {
        logo: '/FlueFlameLogo.svg',
        sidebar: [
            {
                text: 'Introduction',
                collapsible: true,
                items: [
                    { text: 'Introduction', link: '/introduction/intro.md' }
                ]
            },
            {
                text: 'REST',
                collapsible: true,
                items: [
                    { text: 'Basics', link: '/rest/basic.md' }
                ]
            },
            {
                text: 'SignalR',
                collapsible: true,
                items: [
                    { text: 'Basics', link: '/signalr/basic.md' }
                ]
            },
            {
                text: 'gRPC',
                collapsible: true,
                items: [
                    { text: 'Basics', link: '/grpc/basic.md' }
                ]
            }
        ],
        socialLinks: [
            { icon: 'github', link: 'https://github.com/ISBronny/FlueFlame' }
        ],
        editLink: {
            pattern: 'https://github.com/vuejs/vitepress/edit/main/docs/:path'
        }
    }
}