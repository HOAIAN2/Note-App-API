const username = document.querySelector('#username')
const password = document.querySelector('#password')
let token = ''
const loginData = {
    username,
    password
}
const btn = document.querySelector('button')
const btn1 = document.querySelector('#note')
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
            token = json
        })
})
btn1.addEventListener('click', () => {
    fetch('https://localhost:5000/User?username=hoaian_admin', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })
        .then(res => res.json())
        .then(json => {
            console.log(json)
        })
})