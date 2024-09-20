<h1 align="center">Activity Paint<br><span style="font-size:.75em;">$\textsf{\color{red}{(IN DEVELOPMENT)}}$</span></h1>

<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about">About</a></li>
    <li>
        <a href="#technical-overview">Technical overview</a>
        <ul>
          <li><a href="#core">Core</a></li>
          <li><a href="#web">Web</a></li>
          <li><a href="#mobile">Mobile</a></li>
          <li><a href="#cli">CLI</a></li>
        </ul>
    </li>
    <li>
      <a href="#getting-started">Getting started</a>
      <ul>
        <li><a href="#docker">Docker</a></li>
      </ul>
    </li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

## About

Activity Paint is a tool that allows you to easily create an artificial commit history shown on GitHub's profile page. The main idea comes from seeing a picture of GitHub's activity history depicting a Minecraft creeper.

## Technical overview

The libraries and technologies used to develop the application were a bit unconventional. I wanted to experiment a bit with source generators and new features introduced between .NET 6 and .NET 8.

### Core

In order to make the whole architecture clean and readable, I chose to use [`Hexagonal Architecture`](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software)) with most of the folder structure using [`Vertical Slicing`](https://en.wikipedia.org/wiki/Vertical_slice). To maintain the high performance of the app, as the heart of the system, I decided to use the source-generated [`Mediator`](https://github.com/martinothamar/Mediator) implementation. Also, the [`Mapperly`](https://github.com/riok/mapperly) library has been used as a default mapping framework to allow for close to native performance of the mapper.\
To avoid writing separate UI for every platform, a single common `Razor Component Library` project has been created that contains most of the UI elements.

### Web

The main assumption was that the application should work mostly client-side and easily consume UIs used on other platforms. Because of that, the `Blazor WebAssembly` project has been created.

### Mobile

To integrate with the existing UIs, and given the fact that Xamarin has been deprecated, the `MAUI Blazor Hybrid` has been chosen as the best option.

### CLI

The assumption was to use a library that supports both CLI creation and nice console outputs. It was decided to go with the [`Spectre.Console`](https://github.com/spectreconsole/spectre.console)

## Getting started

### Docker

If you want to test the application but do not want to bother with the whole setup, the Docker will be a perfect choice! The Web project has Docker support implemented. All you need to do is follow these steps:

1. Have Docker up-and-running (see: https://www.docker.com/get-started/).
2. Open a terminal in the projects directory.
3. Run:
```bash
# Build docker image
docker build -t activitypaint:latest .

# Create and start a new container at port 8080
docker run -p 8080:80 activitypaint:latest
```
4. Open http://localhost:8080 in the browser.

## [License](/./LICENSE)

```
MIT License

Copyright (c) 2024 Mateusz Ciągło

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```