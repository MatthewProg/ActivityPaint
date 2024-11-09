<h1 align="center">Activity Paint</h1>

<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about">About</a></li>
    <li>
        <a href="#technical-overview">Technical overview</a>
        <ul>
          <li><a href="#development">Development</a></li>
          <ul>
            <li><a href="#core">Core</a></li>
            <li><a href="#web">Web</a></li>
            <li><a href="#mobile">Mobile</a></li>
            <li><a href="#cli">CLI</a></li>
          </ul>
          <li><a href="#testing">Testing</a></li>
          <ul>
            <li><a href="#unit-tests">Unit tests</a></li>
            <li><a href="#integration-tests">Integration tests</a></li>
            <li><a href="#e2e-tests">E2E tests</a></li>
          </ul>
          <li><a href="#cicd">CI/CD</a></li>
        </ul>
    </li>
    <li>
      <a href="#getting-started">Getting started</a>
      <ul>
        <li><a href="#docker">Docker</a></li>
      </ul>
    </li>
    <li><a href="#example">Example</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

## About

Activity Paint is a tool that allows you to easily create an artificial commit history shown on GitHub's profile page. The main idea comes from seeing a picture of GitHub's activity history depicting a Minecraft creeper.

## Technical overview

### Development
The libraries and technologies used to develop the application were a bit unconventional. I wanted to experiment a bit with source generators and new features introduced between .NET 6 and .NET 8.

#### Core

In order to make the whole architecture clean and readable, I chose to use [`Hexagonal Architecture`](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software)) with most of the folder structure using [`Vertical Slicing`](https://en.wikipedia.org/wiki/Vertical_slice). To maintain the high performance of the app, as the heart of the system, I decided to use the source-generated [`Mediator`](https://github.com/martinothamar/Mediator) implementation. Also, the [`Mapperly`](https://github.com/riok/mapperly) library has been used as a default mapping framework to allow for close to native performance of the mapper.\
To avoid writing separate UI for every platform, a single common `Razor Component Library` project has been created that contains most of the UI elements.

#### Web

The main assumption was that the application should work mostly client-side and easily consume UIs used on other platforms. Because of that, the `Blazor WebAssembly` project has been created.

#### Mobile

To integrate with the existing UIs, and given the fact that Xamarin has been deprecated, the `MAUI Blazor Hybrid` has been chosen as the best option.

#### CLI

The assumption was to use a library that supports both CLI creation and nice console outputs. It was decided to go with the [`Spectre.Console`](https://github.com/spectreconsole/spectre.console)

### Testing

The tests cover the most important bits of the application and ensure they work correctly. Common for all tests is naming convention (`MethodName_StateUnderTest_ExpectedBehaviour`) and the `Arrange Act Assert` pattern. Also, all tests use the `XUnit` framework and [`FluentAssertions`](https://github.com/fluentassertions/fluentassertions) for cleaner test assertions. All test projects are stored in the `test` directory.

#### Unit tests

The tests are focused on testing very specific aspects of code and only a small pieces of it. They check all kinds of the business logic (model validations included). The test project names end with `*.Tests` and could be found in the `Unit` solution folder.

#### Integration tests

The tests validate the integration between the app and any external system (e.g. app <-> database, app <-> file system). Tests operate on real resources, not mocks. However, for convenience, they support automatic setup and cleanup of all the temporary resources created before and during testing. The test project names end with `*.IntegrationTests` and could be found in the `Integration` solution folder.

#### E2E tests

The main purpose of the end-to-end tests is to validate that the app runs the same in all the supported browsers. Instead of focussing on the very specific case, it runs real-world scenarios that usually test a whole application workflow. To help with the browser integration, the [`Puppeteer Sharp`](https://github.com/hardkoded/puppeteer-sharp) library has been used. The test project names end with `*.E2ETests` and could be found in the `E2E` solution folder. Additionally, the `*.E2EServer` project has been created to enable running and serving the application from the memory using the `WebApplicationFactory`.

### CI/CD

As the repository is hosted on GitHub, the natural choice was to use the GitHub Actions. The project at the moment has only a single pipeline named [`[CI/CD] Release pipeline`](https://github.com/MatthewProg/ActivityPaint/actions/workflows/pipeline-publish.yml). It is configured to run multiple stages (build, test, publish), each of them could be triggered manually from GitHub's UI.

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

## Example

If you would like to check how the generated activity looks in practice, feel free to take a look at [my 2022 activity graph](https://github.com/MatthewProg?tab=overview&from=2022-12-01&to=2022-12-31#:~:text=Contribution%20activity) and the [`ActivityPaintDemo`](https://github.com/MatthewProg/ActivityPaintDemo) repository.

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