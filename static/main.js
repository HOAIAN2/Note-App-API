const username = document.querySelector('#username')
const password = document.querySelector('#password')
const token = {
    accessToken: "",
    refreshToken: "",
}
const loginData = {
    username,
    password
}
const btn = document.querySelector('button')
const btn1 = document.querySelector('#note')
const btn2 = document.querySelector('#note1')
const btn3 = document.querySelector('#note2')
const title = document.querySelector('#title')
const content = document.querySelector('#content')
const noteData = {
    title,
    content
}
btn.addEventListener('click', () => {
    loginData.username = username.value
    loginData.password = password.value
    fetch('https://localhost:5000/User/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData)
    })
        .then(res => res.json())
        .then(json => {
            console.log(json)
            token.accessToken = json.accessToken
            token.refreshToken = json.refreshToken
        })
})
btn1.addEventListener('click', () => {
    fetch('https://localhost:5000/User?username=hoaian_admin', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token.accessToken}`
        }
    })
        .then(res => res.json())
        .then(json => {
            console.log(json)
        })
})
btn2.addEventListener('click', () => {
    noteData.title = title.value
    noteData.content = content.value
    console.log(noteData)
    fetch('https://localhost:5000/Note', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token.accessToken}`
        },
        body: JSON.stringify(noteData)
    }).then(res => res.json())
        .then(json => {
            console.log(json)
        })
})
btn3.addEventListener('click', () => {
    fetch('https://localhost:5000/Note', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token.accessToken}`
        },
    }).then(res => res.json())
        .then(json => {
            console.log(json)
        })
})