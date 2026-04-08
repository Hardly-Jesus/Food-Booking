const params = new URLSearchParams(window.location.search);
const id = params.get("id");

const btnEliminar = document.getElementById("btnEliminar");



btnEliminar.addEventListener("click", async () => {
    try {

        const token = localStorage.getItem("token");

        await axios.delete(
            `${config.API_URL}/Api/v1/Plato/delete-plato/${id}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );

     
        window.location.href = "/Assets/view/Propietario/plato/plato.html";

    } catch (err) {
        console.log("Error al eliminar el plato", err);
    }
});