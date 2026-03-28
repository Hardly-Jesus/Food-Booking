const formAddPlato = document.getElementById("form-add-plato");
const inputName= document.getElementById("Name");
const inputDescripcion = document.getElementById("descripcion");
const inputImagen = document.getElementById("imagen");
const inputPrecio = document.getElementById("precio");
const inputCategoria = document.getElementById("categoria");



formAddPlato.addEventListener("submit",function(e){
e.preventDefault();


        const nombre = inputName.value;
        const descripcion = inputDescripcion.value;
        const precio = inputPrecio.value;
        const imagen = inputImagen.files[0];
        const categoria = inputCategoria.value;
        
  

        addPlato(nombre,descripcion,imagen,precio,categoria);

});





async function addPlato(nombre,descripcion,imagen,precio,categoria)
{
    try
    {
        
        const formData = new FormData();
        formData.append("Nombre",nombre);
        formData.append("Descripcion",descripcion);
        formData.append("Imagen",imagen);
        formData.append("Precio",precio);
        formData.append("Categoria",categoria);
     
       const token = localStorage.getItem("token"); 

       const result = await axios.post("https://localhost:7039/Api/v1/Plato/add-plato",formData
        ,{
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
       );

       
        return window.location.href = "/Assets/view/Propietario/plato/plato.html";

    }catch(err){
        
        console.log("Ocurrio un error intentar guardar el resturante " + err);
    }
}


