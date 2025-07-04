<p align="center">
  <img src="logo.png" alt="JCore Logo" height="80"/>
</p>

<h1 align="center">JCore — Johan's Interpreted Language</h1>

<p align="center">
  <b>Fast, minimal scripting language made in C#.</b>  
  <i>MIT licensed, runs anywhere .NET NativeAOT!</i>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/license-MIT-green" alt="MIT License"/>
  <img src="https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue" alt="Platforms"/>
</p>

---

## ✨ Features

✅ Constants & Variables: let, make

✅ Full math expressions: with operator precedence

✅ BigInteger math: no overflow, infinite size!

✅ Simple commands: say, let, make

✅ Function references: store and call functions like variables (supports native and user-defined functions)

✅ Native functions: like jcore.getTime() with function reference and live call support

✅ User-defined functions: def fun name() { ... } with full call support

✅ Proper function/class error reporting: detects undefined functions and non-callable variables

✅ Inline & multiline comments: //, /* ... */

✅ Script files & REPL support

---

## 🚀 Getting Started

### Run a Script

```bash
JCore.exe Examples\hello.jc
Or simply drag and drop a .jc file onto JCore.exe!

Try the REPL
Just run without arguments:

bash
Copy
Edit
JCore.exe
Type your code, press enter — supports say, math, and more.

🧑‍💻 Example
say "Hello, World!"
let x = 12345 * 67890
say x

```
📄 License

JCore is licensed under the [MIT License](LICENSE).

<p align="center"><sub>Made with ❤️ by Johan Raphael Joe</sub></p>
