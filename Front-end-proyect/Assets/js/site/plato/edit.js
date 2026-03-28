const formEditPlato = document.getElementById("form-edit-plato");
const inputDescripcion = document.getElementById("descripcion");
const inputImagen = document.getElementById("imagen");
const inputPrecio = document.getElementById("precio");
const inputCategoria = document.getElementById("categoria"); 

const inputName = document.getElementById("Name");


const params = new URLSearchParams(window.location.search);
const id = params.get("id");


async function getPlatoById(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`https://localhost:7039/Api/v1/Plato/getById/${id}`,{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
       
        
        console.log(result.data);
        llenarFormulario(result.data);
    
    }catch(err){

        console.log("Error al obtener restaurante", err);
    }
}


function llenarFormulario(r) {
    inputName.value = r.nombre;
    inputDescripcion.value = r.descripcion;
    inputPrecio.value = r.precio;
    inputCategoria.value = r.categoria;
}



formEditPlato.addEventListener("submit",function(e){
    e.preventDefault();
     editarPlato();


});



async function editarPlato() {

     try{
         

     const formData = new FormData();

    formData.append("Id", id); 
    formData.append("Nombre", inputName.value);
    formData.append("Descripcion", inputDescripcion.value);
    formData.append("Precio", inputPrecio.value);
    formData.append("Categoria", inputCategoria.value);
    
    if (inputImagen.files.length > 0) {
        formData.append("Imagen", inputImagen.files[0]);
    }

    const token = localStorage.getItem("token");

    await axios.put(
        `https://localhost:7039/Api/v1/Plato/update-plato/${id}`,
        formData,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );


      window.location.href = "/Assets/view/Propietario/plato/plato.html";


     }catch(err)   
     {
        console.log("Ocurrio un error al intentar actualizar el plato" + err);

     }
  
}



document.addEventListener("DOMContentLoaded", () => {
    getPlatoById();
});

