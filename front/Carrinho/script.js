
const url = 'http://localhost:5000/'

fetch(url + 'Carrinho/valorTotal')
    .then((response) => response.json())
    .then(json => {
        document.getElementById('valorTotal').textContent = json.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'})
        document.getElementById('valorTotal').value = json
    });


fetch(url + 'Carrinho/ver')
    .then((response) => response.json())
    .then(json => {
        json.forEach(item => {
            var newDiv = document.createElement('div');
            newDiv.className = 'item';

            newDiv.innerHTML =
                `<div class="item">
        <div class="valor-unidade-item">${item.produto.preco.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'})}</div>
            <div class="valor-total-item">${(item.produto.preco * item.quantidade).toLocaleString('pt-br',{style: 'currency', currency: 'BRL'})}</div>
            <div class="quantidade-item">
                <div class="frame-4">
                <div class="text-wrapper-2">${item.quantidade}</div>
                <div class="frame-5">
                    <img class="img" src="img/drop-up-small-1.svg" />
                    <img class="img" src="img/drop-down-small-1.svg" />
                </div>
                </div>
            </div>
        <div class="overlap">
            <div class="frame-foto-item"><img class="gcq-x" src="${item.produto.imagem}" /></div>
            <div class="icone-cancelar" onclick="cancelarItem(${item.id})">
            <div class="overlap-group"><img class="vector" src="img/vector.svg" /></div>
            </div>
        </div>
        <div class="text-wrapper-3">${item.produto.nomeProduto}</div>
    </div>`;

            document.getElementById('container').appendChild(newDiv);
        });
    })
    .catch(error => console.error('Error fetching data:', error));


async function cancelarItem(id) {
    var response = await fetch(url + 'Carrinho/apagar?id=' + id,
        { method: 'DELETE', })

    if (response.ok) { location.reload(true); }
}

function voltarPaginaInicial() {
    location.href = "../index/index.html"
}

function voltarLogin() {
    location.href = "../cadastro-login/index.html";
}

async function finalizarCompra() {
    if (!validaQtdItens()) {
        alert('Carrinho vazio');
        voltarPaginaInicial();
        return
    }

    if (!validaToken()) {
        voltarLogin();
        return
    }

    try {
        var token = sessionStorage.getItem('token');
        if (token == null) {
            alert('Cliente não autenticado. Faça novamente o login.')
            voltarLogin();
            return
        }

        var Cliente = JSON.parse(atob(token.split('.')[1]));
        var idCliente = Cliente.clienteId
        var cupom = buscarCupom();

        var response = await
            fetch(`${url}Pedido/criarNovo?idCliente=${idCliente}&cupom=${cupom}`,
                {
                    method: 'POST',
                    headers: { 'Authorization': 'Bearer ' + token }
                })

        if (response.ok) {
            alert('Pedido realizado com sucesso.')
            location.reload(true);
        }
        else {
            alert('Cliente não autenticado. Faça novamente o login.')
            voltarLogin();
        }
    } catch {
        alert('Cliente não autenticado. Faça novamente o login.')
        voltarLogin();
    }
}

function validaQtdItens() {
    var qtdItens = document.getElementsByClassName('valor-unidade-item').length
    if (qtdItens > 0) { return true }
    else { return false }
}

async function validaToken() {
    try {
        var token = sessionStorage.getItem('token');
        const response = await fetch(url + 'Auth/validaToken',
            {
                method: 'GET',
                headers: { 'Authorization': 'Bearer ' + token }
            })

        if (response.ok) { return true }
        else { return false }

    } catch {
        return false
    }
}

function buscarCupom() {
    try {
        var cupom = document.getElementById('cupom').value;

        if (cupom.toUpperCase() == 'CUPOM10') {
            return 0.1
        }

        return 0;
    } catch {
        return 0;
    }
}

function aplicarCupom() {
    var valorText = document.getElementById('valorTotal').value;
    var valor = Number(valorText);
    var desconto = buscarCupom();

    if (desconto <= 0) {
        alert('Cupom inválido.')
        return
    }
    var valorAjustado = valor * (1 - desconto);

    document.getElementById('valorDesconto').textContent = valorAjustado.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'});
}