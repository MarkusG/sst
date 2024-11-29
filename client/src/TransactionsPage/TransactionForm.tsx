import { Field, Form, Formik } from "formik";
import * as yup from "yup";

export interface TransactionFormValues {
    timestamp: Date,
    amount: number,
    description: string,
    account: string,
    category?: string
}

export interface TransactionRawFormValues {
    timestamp: string,
    amount: string,
    description: string,
    account: string,
    category: string
}

export default function TransactionForm() {
    const schema: yup.ObjectSchema<TransactionFormValues> = yup.object({
        timestamp: yup.date()
            .max(new Date(), 'Cannot be in the future')
            .required('Required'),
        amount: yup.number()
            .required('Required')
            .test('nonzero', 'Must be non-zero', x => x !== 0),
        description: yup.string()
            .required('Required'),
        account: yup.string()
            .required('Required'),
        category: yup.string()
    });

    const initialValues: TransactionRawFormValues = {
        timestamp: '',
        amount: '',
        description: '',
        account: '',
        category: '',
    };

    async function submit(values: TransactionRawFormValues) {
        // I'm not sure if I'm not using Formik right or if it's just bad, but we're in the "make it work" stage. I'll make it work better later(tm)
        console.log({
            timestamp: new Date(values.timestamp),
            amount: Number(values.amount),
            description: values.description,
            account: values.account,
            category: values.category
        });
    }

    return (
        <Formik
            initialValues={initialValues}
            validationSchema={schema}
            onSubmit={submit}>
            {props =>
                <Form>
                    <div className="mb-1 grid grid-rows-[min-content,_min-content,_min-content] grid-cols-[240px_8rem_6fr_1fr_2fr,_min-content] gap-1">
                        <label htmlFor="timestamp">Timestamp:</label>
                        <label htmlFor="amount">Amount:</label>
                        <label htmlFor="description">Description:</label>
                        <label htmlFor="account">Account:</label>
                        <label htmlFor="category">Category:</label>
                        <div></div>
                        <Field id="timestamp" name="timestamp" type="datetime-local" className={`border rounded px-1 ${props.touched.timestamp && props.errors.timestamp ? 'border-red-500' : ''}`}/>
                        <Field id="amount" name="amount" type="number" className={`border rounded px-1 ${props.touched.amount && props.errors.amount ? 'border-red-500' : ''}`}/>
                        <Field id="description" name="description" className={`border rounded px-1 ${props.touched.description && props.errors.description ? 'border-red-500' : ''}`}/>
                        <Field id="account" name="account" className={`border rounded px-1 ${props.touched.account && props.errors.account ? 'border-red-500' : ''}`}/>
                        <Field id="category" name="category" className={`border rounded px-1 ${props.touched.category && props.errors.category ? 'border-red-500' : ''}`}/>
                        <button type="submit" className="text-gray-700 mx-2"><i className="fa fa-arrow-right"></i></button>
                        <p className="text-sm text-red-500">{props.touched.timestamp && props.errors.timestamp && props.errors.timestamp}</p>
                        <p className="text-sm text-red-500">{props.touched.amount && props.errors.amount && props.errors.amount}</p>
                        <p className="text-sm text-red-500">{props.touched.description && props.errors.description && props.errors.description}</p>
                        <p className="text-sm text-red-500">{props.touched.account && props.errors.account && props.errors.account}</p>
                        <p className="text-sm text-red-500">{props.touched.category && props.errors.category && props.errors.category}</p>
                    </div>
                </Form>
            }
        </Formik>
    );
}
