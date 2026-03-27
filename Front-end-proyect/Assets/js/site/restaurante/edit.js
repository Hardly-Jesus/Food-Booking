
const inputName= document.getElementById("Name");
const inputDireccion = document.getElementById("direccion");
const inputTelefono = document.getElementById("telefono");
const inputHoraInicio = document.getElementById("HoraInicio");
const inputHoraFin = document.getElementById("HoraFin");
const InputGastronomia = document.getElementById("gastronomia");
const inputImage = document.getElementById("imagen");
const formUpdateRestaurante = document.getElementById("form-update-restaurante");



const params = new URLSearchParams(window.location.search);
const id = params.get("id");





async function getRestauranteByUsuarioId(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get("https://localhost:7039/Api/v1/Restaurante/GetByUserId",{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
        llenarFormulario(result.data);
    
    }catch(err){

        console.log("Error al obtener restaurante", err);
    }
}


function llenarFormulario(r) {
    inputName.value = r.nombre;
    inputDireccion.value = r.direccion;
    inputTelefono.value = r.telefono;
    inputHoraInicio.value = r.horarioInicio;
    inputHoraFin.value = r.horarioFin;
    InputGastronomia.value = r.especialidadGastronomica;
}


formUpdateRestaurante.addEventListener("submit",function(e){
    e.preventDefault();
     editarRestaurante();


});



async function editarRestaurante() {

    const formData = new FormData();

    formData.append("Id", id); 
    formData.append("Nombre", inputName.value);
    formData.append("Direccion", inputDireccion.value);
    formData.append("Telefono", inputTelefono.value);
    formData.append("HorarioInicio", inputHoraInicio.value);
    formData.append("HorarioFin", inputHoraFin.value);
    formData.append("EspecialidadGastronomica", InputGastronomia.value);

    if (inputImage.files.length > 0) {
        formData.append("Imagen", inputImage.files[0]);
    }

    const token = localStorage.getItem("token");

    await axios.put(
        `https://localhost:7039/Api/v1/Restaurante/Update-restaurante/${id}`,
        formData,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );


   return window.location.href = "/Assets/view/Propietario/restaurante/Restaurantes.html";
}


document.addEventListener("DOMContentLoaded", () => {
    getRestauranteByUsuarioId();
});