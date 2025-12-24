const inputCliente = document.getElementById('Cliente');
const clienteId = document.getElementById('ClienteId');
const lista = document.getElementById('autocomplete-list');

inputCliente.addEventListener("input", async function () {
    const nome = this.value;

    if (nome.length < 2) {
        lista.style.display = "none";
        return;
    };

    const response = await fetch(`/Cliente/BuscarClientesInput?nome=${nome}`);
    const listaClientes = await response.json();
    console.log(listaClientes);

    lista.innerHTML = "";

    if (listaClientes.length === 0)
    {
        lista.style.display = "none";
        return;
    }

    lista.style.display = "block";

    listaClientes.forEach(cliente => {
        console.log(cliente.nome)
        const item = document.createElement('div');
        item.classList.add('autocomplete-item');
        item.textContent = cliente.nome;

        item.addEventListener('click', () => {
            selecionarCliente(cliente.id, cliente.nome);
        });

        lista.appendChild(item);
    });
})

async function selecionarCliente(id, nome){
    inputCliente.value = nome;
    clienteId.value = id;
    lista.style.display = "none";

    const response = await fetch(`/Cliente/DadosCompletosCliente?id=${id}`);
    const dadosCliente = await response.json();

    console.log(dadosCliente);

    document.getElementById('CpfCnpj').value = dadosCliente.cpfCnpj || "";
    document.getElementById('Cep').value = dadosCliente.cep || "";
    document.getElementById('Logradouro').value = dadosCliente.logradouro || "";
    document.getElementById('Numero').value = dadosCliente.numero || "";
    document.getElementById('Cidade').value = dadosCliente.cidade || "";
    document.getElementById('Estado').value = dadosCliente.estado || "";
    document.getElementById('Telefone').value = dadosCliente.telefone || "";

    const inputsCliente = document.querySelectorAll('#Cliente, #CpfCnpj, #Cep, #Logradouro, #Numero, #Cidade, #Estado, #Telefone');

    inputsCliente.forEach(dados => {
        dados.setAttribute("readonly", "");
    })

    const textCliente = document.getElementById('text-cliente');
    textCliente.textContent = inputCliente.value;
}

document.addEventListener("click", function (e) {
    if (!inputCliente.contains(e.target)) {
        lista.style.display = "none";
    }
})
