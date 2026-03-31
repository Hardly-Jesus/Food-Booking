const params = new URLSearchParams(window.location.search);
const id = Number(params.get("id"));

const btnEliminar = document.getElementById("btnEliminar");




async function eliminarResena(){

  try{
    
         const token = localStorage.getItem("token");
         const result = await axios.delete(`https://localhost:7039/Api/v1/Reseña/delete-resenia/${id}`,{
          headers: {
            Authorization: `Bearer ${token}`
          }
         });
  
         window.location.href = "/Assets/view/cliente/restaurante/Restaurante.html";

  }catch(err){
    console.log("Ocurrio un error al intentar eliminar la resenia");
  }
}



btnEliminar.addEventListener("click",function(e){
    e.preventDefault();

    eliminarResena();

});