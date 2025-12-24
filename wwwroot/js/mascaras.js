const handlePhone = (event) => {
    let input = event.target
    input.value = phoneMask(input.value)
}
  
const phoneMask = (value) => {
    if (!value) return ""
    value = value.replace(/\D/g,'')
    value = value.replace(/(\d{2})(\d)/,"($1) $2")
    value = value.replace(/(\d)(\d{4})$/,"$1-$2")
    return value
}

const formatReais = () => {
    const inputs = document.querySelectorAll(".input");

    inputs.forEach(input => {
        let valor = parseFloat(input.value);
        if (!isNaN(valor)) {
            input.value = valor.toFixed(2);
        }
    });
};


const formatarMoeda = (value) => {
    value = value.replace(/\D/g, '');
    value = value.replace(/\./g, "");
    value = (value / 100).toFixed(2) + '';
    value = value.replace(".", ",");
    value = value.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
    return value;
}

const formatarInput = (event) => {
    const input = event.target;
    input.value = formatarMoeda(input.value);
}

const inputs = document.querySelectorAll(".material, .servicos, #ValorRecibo");
inputs.forEach(input => {
    input.addEventListener('input', formatarInput);
});

const limparFormatacao = (valor) => {
    if (!valor) return 0;
    valor = valor.replace(/\./g, "");
    valor = valor.replace(",", ".");
    return parseFloat(valor);
}

