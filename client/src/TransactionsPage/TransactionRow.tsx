import React, { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CategoriesResponse, TransactionResponse } from "../Contracts/Responses";
import Amount from "../Amount";

interface TransactionRowProps {
    transaction: TransactionResponse
}

function TransactionRow({ transaction }: TransactionRowProps) {
    const [categorizing, setCategorizing] = useState(false);
    const [category, setCategory] = useState<string | null>(transaction.category ?? null);

    const queryClient = useQueryClient();

    const { data } = useQuery<CategoriesResponse>({
        queryKey: ['categories'],
        queryFn: async () => await fetch('https://localhost:5001/categories')
            .then((res) => res.json())
    });

    const updateMutation = useMutation({
        mutationFn: async (category: string | null) => {
            await fetch(`https://localhost:5001/transactions/${transaction.id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ ...transaction, category: category })
            });
        },
        onSuccess: () => queryClient.invalidateQueries(['transactions'])
    });

    async function categorize(category: string | null) {
        transaction.category = category ?? undefined;
        await updateMutation.mutateAsync(category);
        queryClient.invalidateQueries(['categories']);
        setCategorizing(false);
    }

    async function finishInput() {
        if (!!category?.trim()) {
            await categorize(category.trim());
        }
        else {
            await categorize(null);
        }
    }

    async function blur() {
        await finishInput();
    }

    function input(e: React.ChangeEvent<HTMLInputElement>) {
        setCategory(e.target.value);
    }

    async function keyDown(e: React.KeyboardEvent<HTMLInputElement>) {
        if (e.key === "Enter") {
            await finishInput();
        }
    }

    return (
        <tr className="odd:bg-gray-100">
            <td className="px-1 pl-4">{transaction.timestamp.toLocaleString()}</td>
            <td className="px-1 text-right"><Amount amount={transaction.amount}/></td>
            <td className="px-1">{transaction.description}</td>
            <td className="px-1">{transaction.account}</td>
            <td className="px-1">
                {!categorizing ? <button className={`w-full text-left ${transaction.category ? "" : "text-gray-400"}`} onClick={_ => setCategorizing(true)}>{transaction.category ?? "Add category"}</button>
                    : (
                        <>
                            <input className="bg-[inherit] focus:bg-gray-200 w-full h-full" list="categoryList"
                                value={category ?? undefined}
                                autoFocus
                                onKeyDown={keyDown}
                                onInput={input}
                                onBlur={blur}/>
                            {!!data &&
                            <datalist id="categoryList">
                                {data.categories.map(c => <option value={c}></option>)}
                            </datalist>
                            }
                        </>
                    )}
            </td>
        </tr>
    );
}

export default TransactionRow;
