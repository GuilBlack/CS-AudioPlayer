// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let mode = localStorage.getItem('mode')

document.addEventListener('DOMContentLoaded', function () {
    var lightDarkToggle = document.getElementById('lightDark');

    if (mode === 'light') {
        enableLightMode();
    }

    lightDarkToggle.addEventListener('click', () => {
        mode = localStorage.getItem('mode');
        if (mode === 'light') {
            enableDarkMode()
        } else {
            enableLightMode()
        }
    });
});

function enableDarkMode() {
    var borders = document.querySelectorAll('.border-grey');
    var texts = document.querySelectorAll('.text-dark');
    var backgrounds = document.querySelectorAll('.bg-light');
    document.querySelector('#dark').style.display = 'none';
    document.querySelector('#light').style.display = '';
    

    borders.forEach(e => {
        e.classList.remove('border-grey');
        e.classList.add('border-light');
    });

    texts.forEach(e => {
        e.classList.remove('text-dark');
        e.classList.add('text-light');
    });

    backgrounds.forEach(e => {
        e.classList.remove('bg-light');
        e.classList.add('bg-dark');
    });

    localStorage.setItem('mode', 'dark');
}

function enableLightMode() {
    var borders = document.querySelectorAll('.border-light');
    var texts = document.querySelectorAll('.text-light');
    var backgrounds = document.querySelectorAll('.bg-dark');
    document.querySelector('#light').style.display = 'none';
    document.querySelector('#dark').style.display = '';

    borders.forEach(e => {
        e.classList.remove('border-light');
        e.classList.add('border-grey');
    });

    texts.forEach(e => {
        e.classList.remove('text-light');
        e.classList.add('text-dark');
    });

    backgrounds.forEach(e => {
        e.classList.remove('bg-dark');
        e.classList.add('bg-light');
    });

    localStorage.setItem('mode', 'light');
}