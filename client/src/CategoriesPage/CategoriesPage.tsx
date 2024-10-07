import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CategoryTreeEntryResponse, CategoryTreeResponse } from "../Contracts/Responses";
import Category, { DragOverEvent, DragOverPosition } from "./Category";
import { useState } from "react";
import Dropzone from "../Dropzone";
import LoadingIcon from "../LoadingIcon/LoadingIcon";

function CategoriesPage() {
    const [draggingCategory, setDraggingCategory] = useState<CategoryTreeEntryResponse | null>(null);
    const [dragOverPosition, setDragOverPosition] = useState<DragOverPosition>(null);
    const [dragOverCategory, setDragOverCategory] = useState<CategoryTreeEntryResponse | null>(null);

    const queryClient = useQueryClient();

    const { data, isLoading, error } = useQuery<CategoryTreeResponse>({
        queryKey: ['categories', 'tree'],
        queryFn: async () => fetch('https://localhost:5001/categories/tree')
            .then((res) => res.json())
    });

    const mutation = useMutation({
        mutationFn: async (c: CategoryTreeEntryResponse) => await fetch(`https://localhost:5001/categories/${c.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: c.id,
                name: c.name,
                position: c.position,
                parentId: c.parentId
            })
        }),
        onSuccess: () => queryClient.invalidateQueries(['categories', 'tree'])
    });

    if (isLoading) {
        return (
            <div className="mt-16 w-min mx-auto">
                <LoadingIcon/>
            </div>
        );
    }

    if (!!error) {
        return (
            <div className="p-2">
                <p className="text-xl">{error.toString()}</p>
            </div>
        );
    }


    async function categoryDraggedOver({ category, position }: DragOverEvent) {
        setDragOverPosition(position);
        setDragOverCategory(category);
    }

    async function clearDragOver() {
        setDragOverCategory(null);
        setDragOverPosition(null);
    }

    async function dragEnd(_: React.DragEvent<HTMLDivElement>, c: CategoryTreeEntryResponse) {
        clearDragOver();
        setDraggingCategory(null);

        if (!dragOverCategory || !dragOverPosition)
            return;

        if (dragOverCategory.id == c.id)
            return;

        switch (dragOverPosition) {
            case 'top':
                c.parentId = dragOverCategory.parentId;
                c.position = dragOverCategory.position;
                break;
            case 'mid':
                c.parentId = dragOverCategory.id;
                c.position = 1;
                break;
            case 'bot':
                c.parentId = dragOverCategory.parentId;
                c.position = dragOverCategory.position + 1;
        }

        await mutation.mutateAsync(c);
    }

    return (
        <>
            <h1 className="text-3xl text-center mt-2">Categories</h1>
            <Dropzone onDragLeave={clearDragOver}>
                {data?.categories.map(c => <Category
                                      key={c.id}
                                      entry={c}
                                      last={data.categories.indexOf(c) === data.categories.length - 1}
                                      level={0}
                                      isDragging={!!draggingCategory}
                                      dragOverCategory={dragOverCategory}
                                      dragOverPosition={dragOverPosition}
                                      onDragStart={async (_, data) => setDraggingCategory(data)}
                                      onDragEnd={dragEnd}
                                      onDragOver={categoryDraggedOver}/>)}
            </Dropzone>
        </>
    );
}

export default CategoriesPage;
