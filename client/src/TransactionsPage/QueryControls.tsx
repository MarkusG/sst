import { QueryParameters } from "../QueryParameters";

export interface QueryControlsProps {
    params: QueryParameters,
    onParamsUpdated: (params: QueryParameters) => Promise<any>
}

export default function QueryControls({ params, onParamsUpdated }: QueryControlsProps) {
    async function change(e: React.ChangeEvent<HTMLSelectElement>) {
        const value = e.target.value;
        await onParamsUpdated(new QueryParameters({ ...params, pageSize: Number(value) }));
    }

    return (
        <div className="ml-2 mb-1">
            <label className="mr-1">Page Size:</label>
            <select className="w-14 text-center border rounded"
                value={params.pageSize}
                onChange={change}>
                <option>100</option>
                <option>50</option>
                <option>40</option>
                <option>30</option>
            </select>
        </div>
    );
}
