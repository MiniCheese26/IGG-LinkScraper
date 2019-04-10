# IGG-LinkScraper

![ScraperImage](https://i.imgur.com/oLg7tM0.png "Screenshot")

# Usage

##### `dotnet iggGamesLinksScraper.dll <url> <url2> <url3>...`

# Requirements

* [.net Core 2.2+](https://dotnet.microsoft.com/download)

# What Does It Do?

So IGG Games uses an annoying url shortener, but they also include the actual url in that shortened url. So my tool just parses the page, grabs all the hosters, gives you a list of hosters to choose from and then displays you the links.

# JS Version

I don't really expect anyone to use this tbh, i just created this for fun, here's a simple tampermonkey script that'll replace all links in the page with their real link

```
// ==UserScript==
// @name         IGGGamesLinkReplacer
// @version      1.0
// @description  Replaces all links in IGG-Games with thier actual link
// @author       Loc0DeD
// @include      http*://igg-games.com/*-download.html
// ==/UserScript==
(function() {
    const nodeCollection = document.getElementsByTagName("a");
    for (let i = 0; i < nodeCollection.length; i++) {
        const node = nodeCollection[i];
        if (node.getAttribute("href") != null && node.getAttribute("href").includes("bluemediafiles")) {
            const pattern = /xurl=s?:\/\/([\w\W]+)/gmi;
            const result = pattern.exec(node.getAttribute("href"));
            if (result.length > 1) node.setAttribute("href", "http://" + result[1]);
        }
    }
})();
```

Tried using JS's lambda functions so the if statement wasn't needed but it kept telling me getElementsByTagName doesn't contain .filter() so ¯\_(ツ)_/¯

# Enjoy
