const formAddMesa = document.getElementById("form-add-mesa");
const inputName = document.getElementById("Name");
const inputDescripcion = document.getElementById("descripcion");
const inputCantidadPersona = document.getElementById("cantidadPersona");





formAddMesa.addEventListener("submit",function(e){
     e.preventDefault();


        const nombre = inputName.value;
        const descripcion = inputDescripcion.value;
        const cantidadPersonas = inputCantidadPersona.value;
        
        addMesa(nombre,descripcion,cantidadPersonas);

});



async function addMesa(nombre,descripcion,cantidadPersonas)
{
    try
    {

       const token = localStorage.getItem("token"); 

       const result = await axios.post(`${config.API_URL}/Api/v1/Mesa/Add-mesa`,
        {
          nombre,
          descripcion,
          cantidadPersonas
        }
        ,{
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
       );

        return window.location.href = "/Assets/view/Propietario/mesa/Mesas.html";

    }catch(err){
        
        console.log("Ocurrio un error intentar guardar el resturante " + err);
    }
}


