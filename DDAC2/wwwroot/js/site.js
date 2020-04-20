// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var Scrollbar = window.Scrollbar;

Scrollbar.init(document.querySelector('#my-scrollbar'));

const swup = new Swup(); // only this line when included with script tag

var conv = new showdown.Converter();
var txt = document.getElementById('markdown-content').innerHTML;
document.getElementById('markdown-content').innerHTML = conv.makeHtml(txt);