const url = 'http://localhost:5000/'

function elementosCadastro() {
    let nome = document.getElementById('nomeCadastro').value;
    let email = document.getElementById('emailCadastro').value;
    let endereco = document.getElementById('enderecoCadastro').value;
    let senha = document.getElementById('senhaCadastro').value;

    return {
        nomeCliente: nome,
        Endereco: endereco,
        email: email,
        Senha: senha
    }
}

async function registrar() {
    const response = await fetch(`${url}Cliente/criarNovo`,
        {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(elementosCadastro())
        }
    )

    if (response.ok) { alert('Cadastro realizado com sucesso.') }
    else { alert('Erro no cadastro. Favor tentar novamente.') }
}

async function logar() {
    var user = document.getElementById('emailLogin').value;
    var pass = document.getElementById('senhaLogin').value;

    if (user == "" || pass == "") { alert('Preencha os dados de login') }
    else {
        try {
            const response = await fetch(`${url}Auth/auth?user=${user}&pass=${pass}`,
                { method: 'POST' })

            if (response.ok) {
                response.json().then(x => sessionStorage.setItem('token', x.token))
                alert('Login realizado com sucesso. Faça seus pedidos');
                location.href = "../index/index.html";
            }
            else { alert('Erro no login. Gentileza avaliar suas credenciais e tentar novamente') }
        }
        catch { alert('Erro no login. Gentileza avaliar suas credenciais e tentar novamente.') }
    }
}
