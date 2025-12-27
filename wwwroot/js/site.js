setTimeout(()=> {
    $(".alert").fadeOut("slow", function() {
        $(this).alert("close");
    })
}, 3500)


async function consultaCEP(cep) {
    const response = await fetch(`https://brasilapi.com.br/api/cep/v1/${cep}`);
    if (response.ok) {
        const data = await response.json();
        return data;
    } else {
        throw new Error('CEP não encontrado!');
    }
}

// Preenche os dados com base no CEP digitado

async function preencherDados() {
    const cepInput = document.getElementById('Cep');
    const cep = cepInput.value.replace(/\D/g, ''); // Remove caracteres não numéricos

    if (cep.length === 8) { // Verifica se o CEP tem 8 dígitos
        try {
            const detalhes = await consultaCEP(cep);
            
            // Preenche os campos com os dados recebidos
            document.getElementById('Logradouro').value = detalhes.street || '';
            document.getElementById('Cidade').value = detalhes.city || '';
            document.getElementById('Estado').value = detalhes.state || '';
        } catch (error) {
            alert(error.message); // Mostra um alerta caso ocorra um erro
        }
    } else if (cep.length === 0) { // Se o campo CEP estiver vazio, limpa os campos
        document.getElementById('Logradouro').value = '';
        document.getElementById('Cidade').value = '';
        document.getElementById('Estado').value = '';
    }
}

async function preencherCNPJ() {
    const cnpjInput = document.getElementById('CpfCnpj');
    const cnpj = cnpjInput.value.replace(/\D/g, ''); 
    const elementosForm = document.querySelectorAll('#inputPadrao');

    if (cnpj.length === 14) {
        try {
            const detalhes = await consultaCNPJ(cnpj);
            const clienteInput = document.getElementById('Cliente');
            clienteInput.value = detalhes.razao_social || '';

        } catch (error) {
            alert(error.message); 
        }
    } else if (cnpj.length === 0) { 
        document.getElementById('Cliente').value = '';
        elementosForm.forEach(elemento => {
            elemento.classList.remove('d-none');
        })
        alterarCliente({ target: document.getElementById('Cliente') });
    }
}

async function consultaCNPJ(cnpj) {
    const response = await fetch(`https://brasilapi.com.br/api/cnpj/v1/${cnpj}`);
    if (response.ok) {
        const data = await response.json();
        return data;
    } else {
        throw new Error('CNPJ não encontrado!');
    }
}

const clienteInput = document.getElementById('Cliente');
const textCliente = document.getElementById('text-cliente');



$(document).on("click", ".btnSelecionar", function () {
    $("#modalCliente").modal('hide');
});

document.querySelector(".btnLimpar").addEventListener("click", () => {
    const inputsCliente = document.querySelectorAll('#Cliente, #CpfCnpj, #Cep, #Logradouro, #Numero, #Cidade, #Estado, #Telefone');

    inputsCliente.forEach(input => {
        input.value = "";
        input.removeAttribute("readonly", "");
    })
    textCliente.innerText = "";
})