const params = new URLSearchParams(window.location.search);
const id = params.get("id");

const formEdit = document.getElementById("form-edit-orden");




document.addEventListener("DOMContentLoaded", () => {
  CargarDatos();
    
});




async function CargarDatos(){
  
     const inputFecha = document.getElementById("fecha");
     const inputHora = document.getElementById("hora");
     
     const data = await GetPedido();

     inputFecha.value = data.fecha.split("T")[0];
     inputHora.value = data.hora.substring(0,5);
    
}



async function GetPedido()
{  
    try{
        
   
        const token = localStorage.getItem("token");
        const resultPedido = await axios.get(`${config.API_URL}/Api/v1/Pedido/update-pedido/${id}`,{
            headers: {
                 Authorization: `Bearer ${token}`
            }
        });

        return resultPedido.data;

    }catch(err){

        console.log("Ocurrio un error al obtener el listado de mesas" + err )
    }

}




formEdit.addEventListener("submit",function(e){
   e.preventDefault();

 
        const inputFecha = document.getElementById("fecha");
        const inputHora = document.getElementById("hora");
    
       
        EditPedido(inputFecha.value,inputHora.value);

});




async function EditPedido(fecha,hora) {

    const token = localStorage.getItem("token");
    await axios.put(
        `${config.API_URL}/Api/v1/Pedido/update-pedido/${id}`,
        {fecha,hora},
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );


   return window.location.href = "/Assets/view/cliente/ordenes/Ordenes.html";
}



