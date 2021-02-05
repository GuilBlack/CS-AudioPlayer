var wavesurfer;

document.addEventListener('DOMContentLoaded', function () {
    console.log('bite');
    wavesurfer = WaveSurfer.create({
        container: '#waveform',
        waveColor: '#428bca',
        progressColor: '#31708f',
        height: 120,
        barWidth: 1,
        responsive: true
    });
});

document.addEventListener('DOMContentLoaded', function () {
    var playPause = document.querySelector('#playPause');
    playPause.addEventListener('click', function () {
        wavesurfer.playPause();
    });

    wavesurfer.on('play', function () {
        document.querySelector('#play').style.display = 'none';
        document.querySelector('#pause').style.display = '';
    });
    wavesurfer.on('pause', function () {
        document.querySelector('#play').style.display = '';
        document.querySelector('#pause').style.display = 'none';
    });

    var toggleMute = document.querySelector('#toggle-mute');
    toggleMute.addEventListener('click', () => {
        wavesurfer.toggleMute();
    });

    wavesurfer.on('mute', (m) => {
        if (m) {
            document.querySelector('#unmuted').style.display = 'none';
            document.querySelector('#muted').style.display = '';
        } else {
            document.querySelector('#unmuted').style.display = '';
            document.querySelector('#muted').style.display = 'none';
        }
    });

    var links = document.querySelectorAll('#playlist tr');
    var currentTrack = 0;

    var setCurrentSong = (index) => {
        console.log(index);
        links[currentTrack].style.backgroundColor = null;
        currentTrack = index;
        links[index].style.backgroundColor = "#428bca";
        wavesurfer.load(links[currentTrack].id);
    }

    var skipPrevious = document.getElementById('previous');
    var skipNext = document.getElementById('next');

    skipNext.addEventListener('click', () => {
        setCurrentSong(mod(currentTrack + 1, links.length));
    })

    skipPrevious.addEventListener('click', () => {
        setCurrentSong(mod(currentTrack - 1, links.length));
    })


    Array.prototype.forEach.call(links, (link, index) => {
        link.addEventListener('onClick', function (e) {
            e.preventDefault();
            setCurrentSong(index);
        });
    });

    links.forEach((e, index) => e.addEventListener("click", function () {
        setCurrentSong(index);
    }));

    wavesurfer.on('ready', function () {
        wavesurfer.play();
    });

    wavesurfer.on('error', function (e) {
        console.warn(e);
    });

    wavesurfer.on('audioprocess', function () {
        if (wavesurfer.isPlaying()) {
            var totalTime = wavesurfer.getDuration(),
                currentTime = wavesurfer.getCurrentTime();

            var curentTimeStr = '' + Math.floor(currentTime / 60) + ':' + Math.floor(currentTime) % 60;
            var totalTimeStr = '' + Math.floor(totalTime / 60) + ':' + Math.floor(totalTime) % 60;

            document.getElementById('current-time').innerText = curentTimeStr;
            document.getElementById('total-time').innerText = totalTimeStr;
        }
    });

    // Go to the next track on finish
    wavesurfer.on('finish', function () {
        setCurrentSong((currentTrack + 1) % links.length);
    });

// Load the first track
    setCurrentSong(currentTrack);

});

function mod(n, m) {
    return ((n % m) + m) % m;
}