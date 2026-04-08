const formAddRestaurante = document.getElementById("form-add-restaurante");
const btnSubmit = document.getElementById("btn-restaurante-submit");
const inputName= document.getElementById("Name");
const inputDireccion = document.getElementById("direccion");
const inputTelefono = document.getElementById("telefono");
const inputHoraInicio = document.getElementById("HoraInicio");
const inputHoraFin = document.getElementById("HoraFin");
const InputGastronomia = document.getElementById("gastronomia");
const inputImage = document.getElementById("imagen");
const btnAddRestaurante = document.getElementById("btn-add-restaurante");
const container = document.getElementById("RestauranteContainer");


formAddRestaurante.addEventListener("submit",function(e){
e.preventDefault();


        const nombre = inputName.value;
        const direccion = inputDireccion.value;
        const telefono = inputTelefono.value;
        const horarioInicio = inputHoraInicio.value;
        const horarioFin = inputHoraFin.value;
        const especialidadGastronomica = InputGastronomia.value;
        const imagen = inputImage.files[0];


        addRestaurante(nombre,direccion,telefono,horarioInicio,horarioFin,especialidadGastronomica,imagen);

});



async function addRestaurante(nombre,direccion,telefono,horarioInicio,horarioFin,especialidadGastronomica,imagen)
{
    try
    {
        
        const formData = new FormData();
        formData.append("Nombre",nombre);
        formData.append("Direccion",direccion);
        formData.append("Telefono",telefono);
        formData.append("HorarioInicio",horarioInicio);
        formData.append("HorarioFin",horarioFin);
        formData.append("EspecialidadGastronomica",especialidadGastronomica);
        formData.append("Imagen",imagen);

       const token = localStorage.getItem("token"); 

       const result = await axios.post(`${config.API_URL}/Api/v1/Restaurante/add-restaurante`,formData
        ,{
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
       );

       
        localStorage.setItem("statusSaveRestaurante",result.status); 
        return window.location.href = "/Assets/view/Propietario/restaurante/Restaurantes.html";

    }catch(err){
        
        console.log("Ocurrio un error intentar guardar el resturante " + err);
    }
}


